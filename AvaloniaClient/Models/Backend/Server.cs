using Serilog;
using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AvaloniaClient.Models.Backend
{
    /// <summary>
    /// Класс реализующий взаимодействие с сервером.
    /// Доступ к экзмепляру осуществляется через cвойство
    /// Server.Instance
    /// </summary>
    internal class Server
    {
        #region fields
        private string _host = ConfigurationManager.AppSettings["ServerHost"];
        public string Host { get { return _host; } }
        private int _port = Convert.ToInt32( ConfigurationManager.AppSettings["ServerPort"]);
        public int Port { get { return _port; } }
        private TcpClient _tcpClient;
        public TcpClient Client { get { return _tcpClient; } }
        private StreamReader? Reader = null;
        private StreamWriter? Writer = null;
        private static Server? _instance = null;
        public static Server Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Server();
                }
                return _instance;
            }
        }
        #endregion

        #region constructors
        private Server()
        {
            Connect();
            Log.Debug($"Server {Host}:{Port} connection status - {Client.Connected}");
        }
        #endregion

        #region methods
        /// <summary>
        /// Метод для восстановленеия соеденения с сервером.
        /// </summary>
        public void Reconnect(Action callback)
        {
            int i = 1;
            while(true)
            {
                Log.Debug($"Trying to connect to the server {Host}:{Port} attempt: {i}");
                Connect();
                if (Client.Connected)
                {
                    Log.Debug($"Server {Host}:{Port} connection - OK!");

                    callback();

                    break;

                }
                i++;
                Task.Delay(1000);

            }
        }
        private void Connect()
        {

            try
            {
                _tcpClient = new TcpClient();
                Log.Debug($"Connecting to server {Host}:{Port}");
                Client.Connect(hostname:Host, port:Port); //подключение клиента
                Reader = new StreamReader(Client.GetStream());
                Writer = new StreamWriter(Client.GetStream());

            }
            catch (SocketException ex)
            {
                Log.Debug($"Connection error {Host}:{Port} {ex.Message}");
                Log.Error(ex.Message);

            }
        }
        /// <summary>
        /// Метод для отправки сообщений на сервер.
        /// </summary>
        /// <exception cref="SocketException"></exception>
        public async Task SendMessageAsync(string message)
        {
            Log.Debug($"Sending message {message} to server {Host}:{Port}");

            try
            {
                await Writer.WriteLineAsync(message);
                await Writer.FlushAsync();
            }
            catch (IOException ex)
            {
                Log.Debug($"Connection error {Host}:{Port} {ex.Message}");

                throw new SocketException();

            }
            catch (SocketException ex)
            {
                Log.Debug($"Connection error. {ex.Message}");
                throw new SocketException();
            }


        }
        /// <summary>
        /// Метод для полуения данных отправленных сервером.
        /// </summary>
        /// <returns>Данные полученные от сервера.</returns>
        /// <exception cref="SocketException"></exception>
        public async Task<string> ReceiveMessageAsync()
        {
            Log.Debug($"Receiving message from server {Host}:{Port}");
            try
            {

                // считываем ответ в виде строки
                string? response = await Reader.ReadLineAsync();
                Log.Debug($"Received message from server {response}");

                return response == null ? String.Empty : response;

            }
            catch (IOException ex)
            {
                Log.Debug($"Connection error {Host}:{Port} {ex.Message}");

                throw new SocketException();

            }
            catch (SocketException ex)
            {
                Log.Debug($"Connection error. {ex.Message}");
                throw new SocketException();
            }


        }
        #endregion

        ~Server()
        {
            Log.Debug("Закрытие соеденений.");
            Client.Close();
            Writer?.Close();
            Reader?.Close();
        }

    }
}
