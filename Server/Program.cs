using Models.Logging;
using Server.Configuration;
using Server.Models.Server;

namespace Server
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            new LoggerInit();

            RootSettings Settings = Configuration.ConfigurationManager.Instance.RootSettings;

            ServerObject server = new ServerObject();// создаем сервер
            Task server_task = server.ListenAsync(); // запускаем сервер
            Task.Run(server.SpamProcessAsync); // Запускаем рассылку
            await server_task;
        }
    }
}