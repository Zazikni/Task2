using System.Threading.Tasks;

namespace AvaloniaClient.Models.AnswerManager
{
    /// <summary>
    /// Класс для обработки ответов сервера.
    /// </summary>
    internal static class AnswerManager
    {
        #region methods
        /// <summary>
        /// Обрабатывает ответ сервера содержащий ответ на попытку аутентификации.
        /// </summary>

        public async static Task<ServerResponse?> AccessResponse(string response)
        {
            if(response.Contains('@'))
            {
                //ServerResponse response_obj = new ServerResponse(response);
                return new ServerResponse(response);
                
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Обрабатывает ответ сервера содержащий ответ на попытку регистрации нового пользователя.
        /// </summary>

        public async static Task<ServerResponse?> RegResponse(string response)
        {
            if (response.Contains('@'))
            {
                //ServerResponse response_obj = new ServerResponse(response);
                return new ServerResponse(response);

            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
