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
        /// <param name="response"></param>
        /// <returns></returns>
        public async static Task<bool> Access(string response)
        {
            if(response == "access allowed")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
