using ClientForAPI.Configuration;
using Serilog;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientForAPI.Models.RemoteServices
{
    public class  FileNotFoundError : Exception
    {
        public FileNotFoundError(string message) : base(message)
        {
        }
    }
    internal class APIService
    {
        #region Fields

        private static APIService? _instance = null;

        public static APIService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new APIService();
                }
                return _instance;
            }
        }
        static private readonly HttpClient _client = new HttpClient();

        private string _host = ConfigurationManager.Instance.RootSettings.API.Host;

        public string Host
        { get { return _host; } }

        private int _port = ConfigurationManager.Instance.RootSettings.API.Port;


        public int Port
        { get { return _port; } }

        #endregion Fields

        #region Constructors
        #endregion

        #region Methods
        public async Task<bool> LifeCheck()
        {
            string URL = "http://" + Host + ":" + Port + "/health";

            Log.Debug(URL);

            try
            {
                HttpResponseMessage response = await _client.GetAsync(URL);

                response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();
                Log.Debug($"Response: {response.StatusCode}");

                return response.StatusCode is System.Net.HttpStatusCode.OK;
            }
            catch (HttpRequestException e)
            {
                Log.Error($"GET запрос по адресу URL {URL} не удался.\n Сообщение: {e.Message} ");
                return false;
            }
        }
        public async Task<string> SendFile(string path_to_file = "C:\\Users\\SA\\Downloads\\prcsdfsdf.jpg")
        {
            if (!File.Exists(path_to_file))
            {
                
                Log.Error($"Файл {path_to_file} не найден.");
                throw new FileNotFoundError($"Файл {path_to_file} не найден.");

            }
            string URL = "http://" + Host + ":" + Port + "/file";
            Log.Debug(URL);
            Log.Debug(path_to_file);
            try
            {
                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    var fileStreamContent = new StreamContent(File.OpenRead(path_to_file));
                    multipartFormContent.Add(fileStreamContent, name: "image", fileName: Path.GetFileName(path_to_file));

                    HttpResponseMessage response = await _client.PostAsync(URL, multipartFormContent);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Log.Debug(responseBody);
                    return responseBody;
                }
            }
            catch (HttpRequestException e)
            {
                Log.Error($"POST запрос по адресу URL {URL} не удался.\n Сообщение: {e.Message} ");
                throw e;
            }






//----------------------
 public async Task<bool> GetAnswerFromApi()
        {
            string URL = "http://" + Host + ":" + Port + "/health";

        
                HttpResponseMessage response = await _client.GetAsync(URL);

            while(true){

                if( response.StatusCode is System.Net.HttpStatusCode.OK)
                  {
                    // все ок 

                   }
                 else{// тут поднимается крутилка если она нужна }
                await Task.Delay(//тут задержка);
            }
        }
//----------------------


        }
        #endregion
    }
}
