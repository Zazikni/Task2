using AvaloniaClient.Models.AnswerManager;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace AvaloniaClient.Models.Backend
{
    internal class ConnectionService
    {
        #region fields
        private static ConnectionService? _instance = null;
        public static ConnectionService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConnectionService();
                }
                return _instance;
            }
        }
        private string _host = ConfigurationManager.AppSettings["ServerHost"];
        public string Host { get { return _host; } }
        private int _port = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);
        private int _monitoring_cd = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);
        private int _sendDelayIfNoConnection = Convert.ToInt32(ConfigurationManager.AppSettings["SendingDelayIfNoConnection"]);
        private int _reciveDelayIfNoConnection = Convert.ToInt32(ConfigurationManager.AppSettings["ReadingDelayIfNoConnection"]);
        private int _monitoringDelay = Convert.ToInt32(ConfigurationManager.AppSettings["MonitoringDelay"]);
        public int Port { get { return _port; } }
        private TcpClient _tcpClient;
        public TcpClient Client { get { return _tcpClient; } }
        private StreamReader? Reader = null;
        private StreamWriter? Writer = null;
        private List<string> requests = new List<string>();
        private List<ServerResponse> responses = new List<ServerResponse>();
        private Thread monitoringThread;
        private bool monitoringIsRunning = true;


        #endregion

        #region delegate
        public delegate void WhenConnectionStatusChangeDelegate();
        private WhenConnectionStatusChangeDelegate _delegateChain;


        #endregion

        #region constructor
        private ConnectionService()
        {
            _tcpClient = new TcpClient();
            _delegateChain  += SimpleCallbackWhenConnectionResumed;

        }
        #endregion

        #region methods
        // Метод для добавления обработчиков к делегату.
        public void AddCallback(WhenConnectionStatusChangeDelegate del)
        {
            _delegateChain += del;
        }

        // Метод для удаления обработчиков из делегата.
        public void RemoveCallback(WhenConnectionStatusChangeDelegate del)
        {
            _delegateChain -= del;
        }
        // Метод, вызывающий делегат.
        private void ExecuteCallback()
        {
            // Вызываем делегат.
            _delegateChain?.Invoke();
        }
        public void Start()
        {

            Connect();
            // запускаем новый поток для мониторинга соеденения
            monitoringThread = new Thread(MonitorConnection) { IsBackground = true };
            //monitoringThread.IsBackground = true;
            monitoringThread.Start();
            // запускаем новый поток для получения данных
            Task.Run(() => ReceiveMessageAsync());
            // запускаем новый поток для получения данных
            Task.Run(() => SendMessageAsync());

        }
        public void Stop()
        {
            monitoringIsRunning = false;
            CloseConnection();
            monitoringThread.Join();
            Log.Information("Соеденение закрыто.");
        }
        private void MonitorConnection()

        {
            Log.Information("Мониторинг соеденения запущен.");

            while (monitoringIsRunning)
            {

                if (!Client.Connected) //TODO тут следовало бы сделать какую нибудь более надежную логику проверки соеденения. Но то что есть - тоже будет работать.
                {
                    ExecuteCallback();
                    Log.Information("Соеденение потерянно. Переподклюение...");
                    Connect(); // Попытка переподключения
                }


                Thread.Sleep(_monitoringDelay);
            }
        }
        private void Connect()
        {
            int attempt = 1;
            while (true)
            {
                Log.Debug($"Попытка номер: {attempt} подключиться к серверу {Host}:{Port}");
                try
                {
                    _tcpClient = new TcpClient();
                    Log.Debug($"Connecting to server {Host}:{Port}");
                    Client.Connect(hostname: Host, port: Port); // Подключение клиента
                    Reader = new StreamReader(Client.GetStream());
                    Writer = new StreamWriter(Client.GetStream());
                    Log.Information($"Соеденение с сервером {Host}:{Port} - установлено. Успешная попытка: {attempt}.");
                    ExecuteCallback();



                    break;

                }
                catch (SocketException ex)
                {
                    Log.Debug($"Ошибка подключения {Host}:{Port} {ex.Message}");
                    attempt++;

                }


            }

        }
        private void CloseConnection()
        {
            Client.Dispose();
            if (Reader != null) { Reader.Dispose(); }
            if (Writer != null) { Writer.Dispose(); }
        }
        // получение сообщений
        private async Task ReceiveMessageAsync()
        {
            Log.Information($"Запущен процесс получения сообщений с сервера {Host}:{Port}");


            while (true)
            {
                if (Client.Connected)
                {
                    try
                    {
                        // считываем ответ в виде строки
                        string? message = await Reader.ReadLineAsync();
                        Log.Debug($"C cервера {Host}:{Port} пришло нвовое сообщение");

                        // если пустой ответ, ничего не выводим на консоль
                        if (string.IsNullOrEmpty(message)) continue;
                        Console.WriteLine($"{message}\n");
                        // TODO наполнение списка ответов

                    }

                    catch (Exception ex)
                    {
                        Log.Information($"Ошибка соеденения {Host}:{Port} {ex.Message}");
                        continue;
                    }

                }
                else
                {
                    await Task.Delay(_reciveDelayIfNoConnection);
                }

            }
        }
        public async Task SendMessageAsync()
        {
            Log.Information($"Запущен процесс отправки сообщений на сервер {Host}:{Port}");

            string message;

            while (true)
            {
                if (Client.Connected && requests.Count > 0)
                {
                    message = requests[0];
                    Log.Information($"Отправка сообщения {message}");
                    try
                    {
                        await Writer.WriteLineAsync(message);
                        await Writer.FlushAsync();
                    }
                    catch (Exception ex)
                    {
                        Log.Information($"Ошибка соеденения {Host}:{Port} {ex.Message}");
                        continue;
                    }

                    requests.Remove(message);


                }
                else
                {
                    await Task.Delay(_sendDelayIfNoConnection);
                }
            }

        }
        private void SimpleCallbackWhenConnectionResumed()
        {
            Log.Debug("SimpleCallbackWhenResumed");
        }
        public void Add(string message)
        {
            requests.Add(message);
        }
        #endregion

    }
}
