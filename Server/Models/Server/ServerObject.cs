using Models.Database;
using Serilog;
using Server.Models.Client;
using System.Net;
using System.Net.Sockets;
using ConfigurationManager = Server.Configuration.ConfigurationManager;

namespace Server.Models.Server
{
    internal class ServerObject
    {
        #region Fields
        private IDatabase _database = DatabasePostgreSql.Instance;

        public IDatabase Database
        { get { return _database; } }

        private TcpListener tcpListener; // сервер для прослушивания
        private List<ClientObject> clients = new List<ClientObject>(); // все подключения
        #endregion Fields

        #region Constructor
        public ServerObject()
        {
            if (PortChecker.IsPortAvailable(ConfigurationManager.Instance.RootSettings.Server.Port) == false)
            {
                Log.Error($"Порт {ConfigurationManager.Instance.RootSettings.Server.Port} - занят другим приложением.");
                Environment.Exit(0);
            }
            else
            {
                Log.Debug($"Порт {ConfigurationManager.Instance.RootSettings.Server.Port} - доступен.");
                tcpListener = new TcpListener(IPAddress.Any, ConfigurationManager.Instance.RootSettings.Server.Port);
            }
        }
        #endregion Constructor

        #region Methods

        public async void SpamProcessAsync()
        {
            Console.WriteLine("Рассылка - запущена.");
            int timeout = ConfigurationManager.Instance.RootSettings.MessageSending.SpamTimeout;

            string message = "0000@000@Какая-то история о Незнайке.";
            while (true)
            {
                if (clients.Count != 0 & clients.Any(c => c.SpamAllowed))
                {
                    Console.WriteLine("Рассылка.");
                    Log.Debug("Рассылка.");

                    await BroadcastMessageAsync(message);
                    Console.WriteLine("Рассылка завершена.");
                    Log.Debug("Рассылка завершена.");

                    await Task.Delay(timeout);
                }
                else
                {
                    await Task.Delay(timeout);
                }
            }
        }

        // прослушивание входящих подключений
        protected internal async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();
                Log.Information($"Сервер запущен. Сокет: {tcpListener.LocalEndpoint} Ожидание подключений...") ;
                Console.WriteLine($"Сервер запущен. Сокет: {tcpListener.LocalEndpoint} Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    clients.Add(clientObject);
                    Log.Information($"Новое подключение от клиента {clientObject.Client.Client.RemoteEndPoint}.");
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
        protected internal async Task BroadcastMessageAsync(string message, string? id = null)
        {
            foreach (var client in clients)
            {
                if (client.Id != id) // если id клиента не равно id отправителя
                {
                    if (client.SpamAllowed) // разрешена массовая отправка
                    {
                        await client.Writer.WriteLineAsync(message); //передача данных
                        await client.Writer.FlushAsync();
                        Log.Information($"Клиент {client.Client.Client.RemoteEndPoint}. Отправлено сообщение {message}");
                        Console.WriteLine($"Клиент {client.Client.Client.RemoteEndPoint}. Отправлено сообщение {message}");
                    }
                    else
                    {
                        Log.Information($"Клиент {client.Client.Client.RemoteEndPoint}. Рассылка запрещена.");
                        Console.WriteLine($"Клиент {client.Client.Client.RemoteEndPoint}. Рассылка запрещена.");
                    }
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
                    Log.Information($"Клиент {client.Client.Client.RemoteEndPoint}. Отправлено сообщение {message}");
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
        #endregion Methods
    }
}