using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaClient.Models.Backend
{
    public class TcpConnectionManager
    {
        private TcpClient tcpClient;
        private string serverIp;
        private int serverPort;
        private bool isRunning;
        private Thread monitoringThread;

        public TcpConnectionManager(string ip, int port)
        {
            serverIp = ip;
            serverPort = port;
            tcpClient = new TcpClient();
        }

        public void Start()
        {
            isRunning = true;
            Connect();
            // Запускаем мониторинг в отдельном потоке
            monitoringThread = new Thread(MonitorConnection) { IsBackground = true };
            monitoringThread.Start();
        }

        private void Connect()
        {
            try
            {
                tcpClient.Connect(serverIp, serverPort);
                Console.WriteLine("Connected to the server.");
                // Здесь можно начать обмен данными с сервером
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to server: " + ex.Message);
            }
        }

        private void MonitorConnection()
        {
            while (isRunning)
            {
                if (tcpClient.Connected)
                {
                    try
                    {
                        // Проверяем, живо ли соединение, пытаясь отправить пустой пакет
                        tcpClient.Client.Send(new byte[1], 0, 0);
                    }
                    catch
                    {
                        Console.WriteLine("Connection lost. Reconnecting...");
                        Connect(); // Попытка переподключения
                    }
                }
                else
                {
                    Console.WriteLine("Not connected. Trying to reconnect...");
                    Connect(); // Попытка переподключения
                }
                Thread.Sleep(5000); // Подождите некоторое время перед следующей проверкой
            }
        }

        public void Stop()
        {
            isRunning = false;
            tcpClient.Close();
            monitoringThread.Join();
            Console.WriteLine("Connection closed.");
        }
    }

}
