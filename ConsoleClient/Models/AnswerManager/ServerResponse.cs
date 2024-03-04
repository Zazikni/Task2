namespace ConsoleClient.Models.AnswerManager
{
    enum StatusCodes
    {
       SPAM = 000,
       OK = 200,
       CREATED = 201,
       ACCEPTED = 202,

       BAD_REQUEST = 400,
       UNAUTHORIZED = 401,
       FORBIDDEN = 403,
       NOT_FOUND  = 404

    }
    internal class ServerResponse:IServerMessage
    {
        #region fields
        private StatusCodes _status;
        public StatusCodes StatusCode { get { return _status; }}
        private string _message;
        public string Message { get { return _message; }}
        private int _id;
        public int Id { get { return _id; }}
        #endregion

        #region constructors
        public ServerResponse(int status_code, string message, int id)
        {
            _status = (StatusCodes)status_code;
            _message = message;
            _id = id;

        }
        #endregion

        #region methods
        public override string ToString()
        {
            return $"{Id}@{StatusCode}@{Message}";
        }
        #endregion
    }
}
