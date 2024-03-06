namespace Server.Configuration
{
    public class RootSettings
    {
        public DatabaseSettings Database { get; set; } = new DatabaseSettings();
        public MessageSendingSettings MessageSending { get; set; } = new MessageSendingSettings();
        public LoggingSettings Logging { get; set; } = new LoggingSettings();
    }

    public class DatabaseSettings
    {
        public string PostgreHost { get; set; } = "localhost";
        public string PostgreDatabaseName { get; set; } = "test_database";
        public string PostgreUsername { get; set; } = "postgres";
        public string PostgrePassword { get; set; } = "85245613";
        public int PostgrePort { get; set; } = 5432;
    }

    public class MessageSendingSettings
    {
        public int SpamTimeout { get; set; } = 5000;
    }

    public class LoggingSettings
    {
        public int LogMaxSizeInBytes { get; set; } = 20000;
    }
}