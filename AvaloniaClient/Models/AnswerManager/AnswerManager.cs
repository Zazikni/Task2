using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using Serilog;

namespace AvaloniaClient.Models.AnswerManager
{
    public class ResponsePharseError : Exception
    {
        public ResponsePharseError(string message) : base(message)
        {
        }
    }
    public class ResponseError : Exception
    {
        public ResponseError(string message) : base(message)
        {
        }
    }
    /// <summary>
    /// Класс для обработки ответов сервера.
    /// </summary>
    internal static class AnswerManager
    {
        #region methods
        /// <summary>
        /// Обрабатывает ответ сервера содержащий ответ на попытку аутентификации.
        /// </summary>

        public async static Task<ServerResponse> GetResponse(string response)
        {
            Log.Debug($"Разбор ответа сервера {response}");
            if(response.Contains('@'))
            {
                string[] response_data = response.Split('@');
                try
                {
                    int status_code = Convert.ToInt32(response_data[0]);
                    string message = response_data[1];
                    int id = Convert.ToInt32(response_data[2]);
                    return new ServerResponse(status_code: status_code, message: message, id: id);

                }
                catch (Exception ex)
                {
                    Log.Error($"Ошибка при разборе ответа сервера {response} {ex.ToString}");
                    throw new ResponsePharseError($"Ошибка при разборе ответа: {response}");
                }
                
            }
            else
            {
                Log.Error($"Ошибка при разборе ответа сервера {response} Неизвестный ответ.");

                throw new ResponseError($"Ошибка при разборе ответа: {response} Неизвестный ответ.") ;
            }
        }
       
        #endregion
    }
}
