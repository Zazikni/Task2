using Models.Security;
using Server.Models.Server;
using System.Net.Sockets;

namespace Server.Models.Client
{
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
        private bool _spamAllowed = false;
        public bool SpamAllowed
        {
            get
            {
                return _spamAllowed;
            }
        }
        protected internal string Id { get; } = Guid.NewGuid().ToString();
        protected internal StreamWriter Writer { get; }
        protected internal StreamReader Reader { get; }

        TcpClient _client;
        public TcpClient Client { get { return _client; } }
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
                        _isAuthorized = await AuthenticationManager.AccessAllowed(login: userData[0], password: userData[1], database: server.Database);
                    }

                    catch (IndexOutOfRangeException)
                    {

                        await server.SinglecastMessageAsync("Ожидаются данные в формате login@password", Id);


                    }

                    if (IsAuthorized)
                    {
                        login = userData[0];
                        password = userData[1];
                        await server.SinglecastMessageAsync($"acess allowed", Id);

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
                        Console.WriteLine($"Получены данные от клиента {Client.Client.RemoteEndPoint}. Данные: {message}");
                        if (message == "spam")
                        {

                            _spamAllowed = !_spamAllowed;
                            Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint}. Рассылка = {SpamAllowed}");

                        }

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
