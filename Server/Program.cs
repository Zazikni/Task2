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
            //IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("Configuration/appsettings.json", false, true);
            //IConfigurationRoot root = builder.Build();

            //Console.WriteLine($"Hello, {root["weather"]} world!");
            //root["weather"] = "sdfsdsdsfsssd";
            //Console.WriteLine($"Hello, {root["weather"]} world!");


            RootSettings Settings = Configuration.ConfigurationManager.Instance.RootSettings;
            
            Console.WriteLine($"Hello, {Settings.Database.PostgrePort} world!");

            new LoggerInit();
            ServerObject server = new ServerObject();// создаем сервер
            Task server_task = server.ListenAsync(); // запускаем сервер
            Task.Run(server.SpamProcessAsync); // Запускаем рассылку
            await server_task;
        }
    }
}   