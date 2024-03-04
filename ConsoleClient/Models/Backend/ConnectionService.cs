using ConsoleClient.Models.AnswerManager;
using Serilog;
using System.Configuration;
using System.Diagnostics;
using System.Net.Sockets;

namespace ConsoleClient.Models.Backend
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
        private int _sendDelayIfNoConnection = Convert.ToInt32(ConfigurationManager.AppSettings["SendingDelayIfNoConnection"]);
        private int _reciveDelayIfNoConnection = Convert.ToInt32(ConfigurationManager.AppSettings["ReadingDelayIfNoConnection"]);
        private int _monitoringDelay = Convert.ToInt32(ConfigurationManager.AppSettings["MonitoringDelay"]);
        //private int _answerTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["MonitoringDelay"]);
        private int _responseIterationDelay = Convert.ToInt32(ConfigurationManager.AppSettings["ResponseIterationDelay"]);
        public int Port { get { return _port; } }
        private TcpClient _tcpClient;
        public TcpClient Client { get { return _tcpClient; } }
        private StreamReader? Reader = null;
        private StreamWriter? Writer = null;
        private List<ServerRequest> requests = new List<ServerRequest>();
        private List<ServerResponse> responses = new List<ServerResponse>();
        public List<ServerResponse> Responses { get { return responses; } }
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

                        // если пустой ответ, пропускаем
                        if (string.IsNullOrEmpty(message)) continue;
                        try
                        {
                            Log.Debug($"Сообщение {message}");
                            ServerResponse response =  await AnswerManager.AnswerManager.GetResponse(message);
                            responses.Add(response);
                            Console.WriteLine($"{message}");
                        }
                        catch 
                        {
                            continue;
                        }



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

            ServerRequest request;

            while (true)
            {
                if (Client.Connected && requests.Count > 0)
                {
                    request = requests[0];
                    Log.Information($"Отправка сообщения {request.ToString()}");
                    try
                    {
                        await Writer.WriteLineAsync(request.ToString());
                        await Writer.FlushAsync();
                    }
                    catch (Exception ex)
                    {
                        Log.Information($"Ошибка соеденения {Host}:{Port} {ex.Message}");
                        continue;
                    }

                    requests.Remove(request);


                }
                else
                {
                    await Task.Delay(_sendDelayIfNoConnection);
                }
            }

        }
        private void SimpleCallbackWhenConnectionResumed()
        {
            Log.Debug("Статус соеденения изменился - обработка события делегатом.");
        }
        public void AddRequest(ServerRequest request)
        {
            requests.Add(request);
        }
        public async Task<ServerResponse> GetResponseAsync(int response_id, TimeSpan timeout)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            // TODO стоило бы как-то оптимизировать этот метод
            while (true)
            {
                foreach (ServerResponse response in responses)
                {
                    if (response.Id == response_id)
                    {
                        responses.Remove(response);
                        return response;
                    }

                }
                if (stopwatch.Elapsed > timeout)
                {
                    stopwatch.Stop();
                    throw new TimeoutException($"Ожидание ответа привысило {timeout.TotalSeconds} секунд.");

                }
                await Task.Delay(_responseIterationDelay);

            }
            


            }
        #endregion

    }
}
