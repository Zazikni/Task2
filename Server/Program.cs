using Models.Logging;
using Server.Models.Server;
using Microsoft.Extensions.Configuration;
using Server.Configuration;


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