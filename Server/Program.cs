using Models.Logging;
using Server.Models.Server;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            new LoggerInit();
            ServerObject server = new ServerObject();// создаем сервер
            Task server_task = server.ListenAsync(); // запускаем сервер
            Task.Run(server.SpamProcessAsync); // Запускаем рассылку
            await server_task;


        }








    }
    
}
