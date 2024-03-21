using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Server.Models.Server
{
    internal class PortChecker
    {
        public static bool IsPortAvailable(int port)
        {
            if (port < 1024 || port > 65535)
            {
                Log.Error($"Порт должен быть в диапазоне от 1024 до 65535");
                return false;
            }
            bool isAvailable = true;

            // Пытаемся создать TcpListener на порту.
            TcpListener tcpListener = null;
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, port);

                tcpListener.Start();
            }
            catch (SocketException)
            {
                // Если здесь возникает исключение, значит, порт занят
                isAvailable = false;
            }
            finally
            {
                // Останавливаем прослушивание и освобождаем порт
                if (tcpListener != null)
                {
                    tcpListener.Stop();
                }
            }

            return isAvailable;
        }
    }
}
