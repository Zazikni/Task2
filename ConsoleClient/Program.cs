using Backend;
using System.Net.Sockets;
using System.Text;

namespace ConsoleClient
{
    internal class Program
    {
        /*
        static async Task Main(string[] args)
        {
            string host = "127.0.0.1";
            int port = 8888;
            using TcpClient client = new TcpClient();
            StreamReader? Reader = null;
            StreamWriter? Writer = null;
            Console.WriteLine("Для отправки команды введите ее и нажмите Enter");


            try
            {
                client.Connect(host, port); //подключение клиента
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());
                if (Writer is null || Reader is null) return;

                // запускаем новый поток для получения данных
                Task.Run(() => ReceiveMessageAsync(Reader));
                // запускаем ввод сообщений
                await SendMessageAsync(Writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Writer?.Close();
            Reader?.Close();

            // отправка сообщений
            async Task SendMessageAsync(StreamWriter writer)
            {
                string? userData = Console.ReadLine();

                await writer.WriteLineAsync(userData);
                await writer.FlushAsync();

                while (true)
                {
                    string? message = Console.ReadLine();
                    await writer.WriteLineAsync(message);
                    await writer.FlushAsync();
                }
            }
            // получение сообщений
            async Task ReceiveMessageAsync(StreamReader reader)
            {
                while (true)
                {
                    try
                    {
                        // считываем ответ в виде строки
                        string? message = await reader.ReadLineAsync();
                        // если пустой ответ, ничего не выводим на консоль
                        if (string.IsNullOrEmpty(message)) continue;

                        Print("-->    " + message);//вывод сообщения
                    }

                    catch
                    {
                        break;
                    }
                }
            }
            // чтобы полученное сообщение не накладывалось на ввод нового сообщения
            void Print(string message)
            {
                if (OperatingSystem.IsWindows())    // если ОС Windows
                {
                    var position = Console.GetCursorPosition(); // получаем текущую позицию курсора
                    int left = position.Left;   // смещение в символах относительно левого края
                    int top = position.Top;     // смещение в строках относительно верха
                                                // копируем ранее введенные символы в строке на следующую строку
                    Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
                    // устанавливаем курсор в начало текущей строки
                    Console.SetCursorPosition(0, top);
                    // в текущей строке выводит полученное сообщение
                    Console.WriteLine(message);
                    // переносим курсор на следующую строку
                    // и пользователь продолжает ввод уже на следующей строке
                    Console.SetCursorPosition(left, top + 1);
                }
                else Console.WriteLine(message);
            }


        }*/
        static async Task Main(string[] args)
        {
            new LoggerInit();
            ConnectionService.Instance.Start();
            while (true)
            {
                ConnectionService.Instance.Add(Console.ReadLine());
            }
        }
    }
}
