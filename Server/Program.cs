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
            Task.Run(server.SpamProcessAsync); // Запускаем рассылку
            await server.ListenAsync(); // запускаем сервер
        }



        
        



    }
    
}
