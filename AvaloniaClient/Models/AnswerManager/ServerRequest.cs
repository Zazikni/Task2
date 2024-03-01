using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvaloniaClient.Models.AnswerManager
{
    internal class ServerRequest:IServerMessage
    {
        private string _message;
        public string Message { get { return _message; } }
        private int _id;
        public int Id { get { return _id; } }
        public ServerRequest(string message)
        {
            _message = message;
            _id = new Random().Next(100000000, 999999999);
        }
        public override string ToString()
        {
            return $"{Message}@{Id}";
        }
    }

}
