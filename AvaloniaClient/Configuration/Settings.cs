namespace AvaloniaClient.Configuration
{
    public class RootSettings
    {
        public ConnectionServiceSettings ConnectionService { get; set; } = new ConnectionServiceSettings();
        public LoggingSettings Logging { get; set; } = new LoggingSettings();
    }

    public class ConnectionServiceSettings
    {
        public ServerSettings Server { get; set; } = new ServerSettings();

        public int SendDelayIfNoConnection { get; set; } = 500;
        public int RecieveDelayIfNoConnection { get; set; } = 500;
        public int MonitoringDelay { get; set; } = 2000;
        public int ResponseIterationDelay { get; set; } = 500;

        public class ServerSettings
        {
            public string ServerHost { get; set; } = "127.0.0.1";

            public int ServerPort { get; set; } = 8888;
        }
    }

    public class LoggingSettings
    {
        public int LogMaxSizeInBytes { get; set; } = 20000;
    }
}