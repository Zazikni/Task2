
using Npgsql;


namespace Models.Database
{
    public class DatabaseConnectionError : Exception
    {
        public DatabaseConnectionError(string message) : base(message)
        {
        }
        public DatabaseConnectionError() : base()
        {
        }
    }
}