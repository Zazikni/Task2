using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace AvaloniaClient.Models.AnswerManager
{
    enum StatusCodes
    {
       OK = 200,
       CREATED = 201,
       ACCEPTED = 202,

       UNAUTHORIZED = 401,
       FORBIDDEN = 403,
       NOT_FOUND  = 404

    }
    internal class ServerResponse
    {
        #region fields
        private StatusCodes _status;
        public StatusCodes StatusCode { get { return _status; }}
        private string _message;
        public string Message { get { return _message; }}
        #endregion

        #region constructors
        public ServerResponse(string response)
        {
            string[] response_data = response.Split('@');
            _status = (StatusCodes)Convert.ToInt32(response_data[0]);
            _message = response_data[1];

        }
        #endregion
    }
}
