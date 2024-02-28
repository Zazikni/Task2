using Models.Database;
using System.Net.Sockets;
using System.Net;
using System.Xml.Linq;
using Server.Models.Client;

namespace Server.Models.Server
{
    class ServerObject
    {
        private IDatabase _database = DatabasePostgreSql.Instance;
        public IDatabase Database { get { return _database; } }
        TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888); // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все подключения

        public async void SpamProcessAsync()
        {
            Console.WriteLine("Рассылка - запущена.");

            string message = "Какая-то история о Незнайке.";
            while (true)
            {
                if (clients.Count != 0)
                {
                    Console.WriteLine("Рассылка.");

                    await BroadcastMessageAsync(message);
                    Console.WriteLine("Рассылка завершена.");

                    await Task.Delay(10000);
                }
                else
                {
                    Console.WriteLine("Сообщения не отправлены -  подключений нет.");
                    await Task.Delay(10000);

                }
            }
        }

        // прослушивание входящих подключений
        protected internal async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    clients.Add(clientObject);
                    Console.WriteLine($"Новое подключение от клиента {clientObject.Client.Client.RemoteEndPoint}.");
                    Task.Run(clientObject.ProcessAsync);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject? client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null) clients.Remove(client);
            client?.Close();
        }

        // трансляция сообщения подключенным клиентам
        protected internal async Task BroadcastMessageAsync(string message, string? id = null)
        {
            foreach (var client in clients)
            {
                if (client.Id != id) // если id клиента не равно id отправителя
                {
                    if (client.SpamAllowed) // разрешена массовая отправка
                    {

                        await client.Writer.WriteLineAsync(message); //передача данных
                        await client.Writer.FlushAsync();
                        Console.WriteLine($"Клиент {client.Client.Client.RemoteEndPoint}. Отправлено сообщение {message}");

                    }
                    else
                    {
                        Console.WriteLine($"Клиент {client.Client.Client.RemoteEndPoint}. Рассылка запрещена.");
                    }

                }
            }
        }
        // трансляция сообщения одному клиентам
        protected internal async Task SinglecastMessageAsync(string message, string id)
        {
            foreach (var client in clients)
            {
                if (client.Id == id) // если id клиента равно id отправителя
                {
                    await client.Writer.WriteLineAsync(message); //передача данных
                    await client.Writer.FlushAsync();
                }
            }
        }
        // отключение всех клиентов
        protected internal void Disconnect()
        {
            foreach (var client in clients)
            {
                client.Close(); //отключение клиента
            }
            tcpListener.Stop(); //остановка сервера
        }
    }
}
