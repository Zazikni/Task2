using Models.Database;
using Models.Security;
using Models.Users;
using Npgsql;
using Serilog;
using Server.Models.Server;
using System.Net.Sockets;

namespace Server.Models.Client
{
    internal class ClientObject
    {
        #region Fields

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

        private TcpClient _client;

        public TcpClient Client
        { get { return _client; } }

        private ServerObject server; // объект сервера

        #endregion Fields

        #region Constructors

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

        #endregion Constructors

        #region Methods

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
                        Log.Information($"Получены данные от клиента {Client.Client.RemoteEndPoint}. Данные: {message}");

                        #region Spam
                        if (message.Contains("-spam"))
                        {
                            Log.Debug($"Клиент {Client.Client.RemoteEndPoint} - запросил {(_spamAllowed ? "отключение" : "подключение")} рассылки.");
                            int request_id = Convert.ToInt32(message.Remove(9));

                            if (!IsAuthorized)
                            {
                                Log.Information($"{request_id}@400@Доступ запрещен.");

                                await server.SinglecastMessageAsync($"{request_id}@400@Доступ запрещен.", Id);
                                continue;
                            }
                            else
                            {
                                _spamAllowed = !_spamAllowed;
                                Log.Information($"{request_id}@200@ОК");
                                await server.SinglecastMessageAsync($"{request_id}@200@ОК", Id);
                            }
                        }
                        #endregion

                        #region Auth
                        else if (message.Contains("-auth"))
                        {
                            Log.Information($"Клиент {Client.Client.RemoteEndPoint} - запросил аутентификацию.");
                            Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - запросил аутентификацию.");
                            int request_id = Convert.ToInt32(message.Remove(9));

                            string[] userData = message.Trim().Split('@');

                            try
                            {
                                _isAuthorized = await AuthenticationManager.AccessAllowed(login: userData[2], password: userData[3], database: server.Database);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                Log.Information($"{request_id}@400@Неправильный формат входных данных.");
                                await server.SinglecastMessageAsync($"{request_id}@400@Неправильный формат входных данных.", Id);
                                continue;
                            }
                            catch (DatabaseConnectionError)
                            {
                                Log.Information($"{request_id}@500@Ошибка сервера.");
                                await server.SinglecastMessageAsync($"{request_id}@500@Ошибка сервера.", Id);
                                continue;

                            }
                            if (IsAuthorized)
                            {
                                login = userData[2];
                                password = userData[3];
                                Log.Information($"{request_id}@200@Доступ разрешен.");
                                await server.SinglecastMessageAsync($"{request_id}@200@Доступ разрешен.", Id);
                                Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - прошел аутентификацию.");

                            }
                            else
                            {
                                Log.Information($"{request_id}@403@Доступ запрещен.");
                                await server.SinglecastMessageAsync($"{request_id}@403@Доступ запрещен.", Id);
                                Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - не прошел аутентификацию.");
                            }
                        }
                        #endregion

                        #region Reg
                        else if (message.Contains("-reg"))
                        {
                            Log.Information($"Клиент {Client.Client.RemoteEndPoint} - запросил регистрацию.");
                            Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - запросил регистрацию.");
                            int request_id = Convert.ToInt32(message.Remove(9));
                            string[] userData = message.Trim().Split('@');

                            try
                            {
                                string new_user_name = userData[2];
                                string new_user_login = userData[3];
                                string new_user_password = userData[4];
                                try
                                {
                                    await DatabasePostgreSql.Instance.AddUser(new NewUser(name: new_user_name, login: new_user_login, password: new_user_password));

                                }
                                catch (DatabaseConnectionError)
                                {
                                    Log.Information($"{request_id}@500@Ошибка сервера.");
                                    await server.SinglecastMessageAsync($"{request_id}@500@Ошибка сервера.", Id);
                                    continue;
                                }
                                await server.SinglecastMessageAsync($"{request_id}@201@Пользователь успешно зарегистрирован.", Id);
                                Log.Information($"{request_id}@201@Пользователь успешно зарегистрирован.");

                                Console.WriteLine($"Клиент {Client.Client.RemoteEndPoint} - успешно зарегистрирован.");
                            }
                            catch (IndexOutOfRangeException)
                            {
                                await server.SinglecastMessageAsync($"{request_id}@400@Неправильный формат входных данных.", Id);
                                continue;
                            }
                            catch (NpgsqlException ex)
                            {
                                if (ex.Message.Contains("\"users_login_key\""))
                                {
                                    Log.Information($"{request_id}@400@Логин уже занят.");
                                    await server.SinglecastMessageAsync($"{request_id}@400@Логин уже занят.", Id);
                                    continue;

                                }
                                else
                                {
                                    Log.Information($"{request_id}@400@Неправильный формат данных.");
                                    await server.SinglecastMessageAsync($"{request_id}@400@Неправильный формат данных.", Id);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Information($"{request_id}@400@Неправильный формат входных данных.");
                                await server.SinglecastMessageAsync($"{request_id}@400@Неправильный формат входных данных.", Id);

                                Console.WriteLine(ex.ToString());
                            }
                        }
                        else
                        {
                            await server.SinglecastMessageAsync($"404@404@Ресурс не найден.", Id);
                        }
                        #endregion
                    }
                    catch
                    {
                        message = $"Клиент {Client.Client.RemoteEndPoint} отключился.";
                        Log.Information(message);
                        Console.WriteLine(message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Information(e.Message);
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

        #endregion Methods
    }
}