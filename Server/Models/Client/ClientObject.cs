using Models.Database;
using Models.Security;
using Server.Models.Server;
using System.Net.Sockets;
using Models.Users;
using Npgsql;

namespace Server.Models.Client
{
    class ClientObject
    {
        #region fields
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
        #endregion

        #region constructors
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
        #endregion

        #region methods
        public async Task ProcessAsync()
        {
            try
            {
                string login = String.Empty;
                string password = String.Empty;
                string? message = null;

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message == null) continue;
                        Console.WriteLine($"Получены данные от клиента {Client.Client.RemoteEndPoint}. Данные: {message}");
                        if (message == "-spam")
                        {
                            if (!IsAuthorized)
                            {
                                await server.SinglecastMessageAsync("access denied", Id);
                                continue;
                            }

                            _spamAllowed = !_spamAllowed;
                            Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint}. Рассылка = {SpamAllowed}");

                        }
                        else if(message.StartsWith("-auth "))
                        {
                            Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - запросил аутентификацию.");
                            message = message.Trim().Substring(6);
                            string[] userData = message.Trim().Split('@');


                            try
                            {
                                _isAuthorized = await AuthenticationManager.AccessAllowed(login: userData[0], password: userData[1], database: server.Database);
                            }

                            catch (IndexOutOfRangeException)
                            {
                                await server.SinglecastMessageAsync("400@Ожидаются данные в формате login@password", Id);

                            }
                            if (IsAuthorized)
                            {
                                login = userData[0];
                                password = userData[1];
                                await server.SinglecastMessageAsync($"200@access allowed", Id);

                            }
                            else
                            {
                                await server.SinglecastMessageAsync("403@access denied", Id);
                                Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - не прошел аутентификацию.");
                            }

                        }
                        else if (message.StartsWith("-reg "))
                        {
                            Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - запросил регистрацию.");
                            message = message.Trim().Substring(5);
                            string[] userData = message.Trim().Split('@');


                            try
                            {
                                string new_user_name = userData[0];
                                string new_user_login = userData[1];
                                string new_user_password = userData[2];
                                await DatabasePostgreSql.Instance.AddUser(new NewUser(name:new_user_name, login:new_user_login, password:new_user_password));
                                await  server.SinglecastMessageAsync("201@created", Id);
                                Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - успешно зарегистрирован.");




                            }
                            catch (IndexOutOfRangeException)
                            {

                                await server.SinglecastMessageAsync("400@Ожидаются данные в формате name@login@password", Id);
                                continue;

                            }
                            catch(NpgsqlException ex)
                            {
                                if (ex.Message.Contains("\"users_login_key\""))
                                {
                                    await server.SinglecastMessageAsync($"400@user alredy exists", Id);

                                }
                                else
                                {
                                    await server.SinglecastMessageAsync($"400@data error", Id);

                                }

                            }
                            catch (Exception ex)
                            {
                                await server.SinglecastMessageAsync($"400@data error", Id);

                                Console.WriteLine(ex.ToString());
                            }


                        }
                        else
                        {
                            await server.SinglecastMessageAsync("404@no such command", Id);

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
        #endregion
    }
}
