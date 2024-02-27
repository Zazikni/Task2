using System.Net.Sockets;
using System.Net;
using System.Text;
using Models.Logging;
using static System.Net.Mime.MediaTypeNames;
using Models.Database;
using Models.Security;
using System.Configuration;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            new LoggerInit();
            ServerObject server = new ServerObject();// создаем сервер
            await server.ListenAsync(); // запускаем сервер
        }


        class ServerObject
        {
            private IDatabase _database = DatabasePostgreSql.Instance;
            public IDatabase Database { get { return _database; } }
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888); // сервер для прослушивания
            List<ClientObject> clients = new List<ClientObject>(); // все подключения

            // прослушивание входящих подключений
            protected internal async Task ListenAsync()
            {
                try
                {
                    tcpListener.Start();
                    Console.WriteLine("Сервер запущен. Ожидание подключений...");

                    while (true)
                    {
                        TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                        ClientObject clientObject = new ClientObject(tcpClient, this);
                        clients.Add(clientObject);
                        Console.WriteLine($"Новое подключение от клиента {clientObject.Client.Client.RemoteEndPoint}.");
                        Task.Run(clientObject.ProcessAsync);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Disconnect();
                }
            }
            protected internal void RemoveConnection(string id)
            {
                // получаем по id закрытое подключение
                ClientObject? client = clients.FirstOrDefault(c => c.Id == id);
                // и удаляем его из списка подключений
                if (client != null) clients.Remove(client);
                client?.Close();
            }

            // трансляция сообщения подключенным клиентам
            protected internal async Task BroadcastMessageAsync(string message, string id)
            {
                foreach (var client in clients)
                {
                    if (client.Id != id) // если id клиента не равно id отправителя
                    {
                        await client.Writer.WriteLineAsync(message); //передача данных
                        await client.Writer.FlushAsync();
                    }
                }
            }
            // трансляция сообщения одному клиентам
            protected internal async Task SinglecastMessageAsync(string message, string id)
            {
                foreach (var client in clients)
                {
                    if (client.Id == id) // если id клиента равно id отправителя
                    {
                        await client.Writer.WriteLineAsync(message); //передача данных
                        await client.Writer.FlushAsync();
                    }
                }
            }
            // отключение всех клиентов
            protected internal void Disconnect()
            {
                foreach (var client in clients)
                {
                    client.Close(); //отключение клиента
                }
                tcpListener.Stop(); //остановка сервера
            }
        }
        class ClientObject
        {
            private bool _isAuthorized;
            public bool IsAuthorized
            {
                get
                {
                    return _isAuthorized;
                }
            }
            protected internal string Id { get; } = Guid.NewGuid().ToString();
            protected internal StreamWriter Writer { get; }
            protected internal StreamReader Reader { get; }

            TcpClient _client;
            public TcpClient Client { get {return _client; } }
            ServerObject server; // объект сервера

            public ClientObject(TcpClient tcpClient, ServerObject serverObject)
            {
                
                _client = tcpClient;
                server = serverObject;
                // получаем NetworkStream для взаимодействия с сервером
                var stream = _client.GetStream();
                // создаем StreamReader для чтения данных
                Reader = new StreamReader(stream);
                // создаем StreamWriter для отправки данных
                Writer = new StreamWriter(stream);
            }

            public async Task ProcessAsync()
            {
                try
                {
                    string login = String.Empty;
                    string password = String.Empty;
                    while (!IsAuthorized)
                    {
                        // получаем логин и пароль пользователя
                        string? connString = await Reader.ReadLineAsync();
                        string[] userData = connString.Split('@');
                        try
                        {
                            Console.WriteLine($"Получены данные от клиента {Client.Client.RemoteEndPoint}. Данные: {connString}");
                            _isAuthorized = await Models.Security.AuthenticationManager.AccessAllowed(login: userData[0], password: userData[1], database: server.Database);
                        }

                        catch (IndexOutOfRangeException)
                        {

                            await server.SinglecastMessageAsync("Ожидаются данные в формате login@password", Id);


                        }

                        if (IsAuthorized)
                        {
                            login = userData[0];
                            password = userData[1];
                        }
                        else
                        {
                            await server.SinglecastMessageAsync("Неверные данные.", Id);
                            Console.WriteLine($"Введенные данные от клиента {Client.Client.RemoteEndPoint} - не подошли.");
                        }


                    }

                    Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - авторизовался под логином:{login}.");
                    string? message = null;

                    // в бесконечном цикле получаем сообщения от клиента
                    while (true)
                    {
                        try
                        {
                            message = await Reader.ReadLineAsync();
                            if (message == null) continue;
                            message = $"{login}: {message}";
                            Console.WriteLine(message);
                            await server.BroadcastMessageAsync(message, Id);
                        }
                        catch
                        {
                            message = $"{login} отключился.";
                            Console.WriteLine(message);
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    // в случае выхода из цикла закрываем ресурсы
                    server.RemoveConnection(Id);
                }
            }
            // закрытие подключения
            protected internal void Close()
            {
                Writer.Close();
                Reader.Close();
                _client.Close();
            }
        }



    }
    
}
