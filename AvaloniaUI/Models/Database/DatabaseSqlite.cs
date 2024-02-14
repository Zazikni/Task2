using Microsoft.Data.Sqlite;
using System;
using System.IO;
using Serilog;
using AvaloniaUI.Models.Users;
using AvaloniaUI.Models.Security;
using System.Threading.Tasks;

namespace AvaloniaUI.Models.Database
{


    internal class DatabaseSqlite :IDatabase
    {
        #region fields
        private string _connectionString;
        private SqliteConnection _connection;
        private string _database_path;
        
        public string ConnectionString { get { return _connectionString; } }
        public SqliteConnection Connection { get { return _connection; } }
        public string DatabsePath { get; }
        #endregion

        #region constructors
        public DatabaseSqlite(
            string DataSource = "project.db",
            SqliteOpenMode Mode = SqliteOpenMode.ReadWriteCreate,
            SqliteCacheMode Cache = SqliteCacheMode.Default,
            string? Password = null,
            bool? ForeignKeys = null
            )
        {
            SqliteConnectionStringBuilder _connection_string_builder  = new SqliteConnectionStringBuilder();
            _connection_string_builder.DataSource = DataSource;
            _connection_string_builder.Mode = Mode;
            _connection_string_builder.Cache = Cache;
            
            if ( Password != null ) { _connection_string_builder.Password = Password; }
            if ( ForeignKeys != null ) { _connection_string_builder.ForeignKeys = ForeignKeys; }
            _connectionString = _connection_string_builder.ToString();
            _connection = new SqliteConnection(_connectionString);
            _database_path = DataSource;
            _connection.Open();
            Log.Debug($" !File.Exists(DatabsePath)  {!File.Exists(DatabsePath)}");
            if (!File.Exists(DatabsePath))
            {
                _init_db();

            }




        }
        #endregion

        #region methods
        private async Task _init_db()
        {

            SqliteCommand _command = new SqliteCommand();
            _command.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, login TEXT NOT NULL UNIQUE, name TEXT NOT NULL, password TEXT NOT NULL CHECK(length(\"password\") == 64 ))";
            _command.Connection = Connection;
            _command.ExecuteNonQuery();
            await AddUser(new NewUser(name: "Nikita", login: "zazik", password: "K2342^%&KNKNKJw4cw3c4wr"));
            await AddUser(new NewUser(name: "Oleg", login: "katsd", password: "712876asKasdsassdr"));
            await AddUser(new NewUser(name: "Billy", login: "billy212", password: "45466&%#dt56asdasd."));
            await GetUser("zazik");

        }
        public async Task AddUser( NewUser user)
        {
            try
            {
                SqliteCommand _command = new SqliteCommand();
                _command.CommandText = "INSERT INTO Users (login, name, password) VALUES (@login, @name, @password)";
                _command.Parameters.AddWithValue("@login", user.Login);
                _command.Parameters.AddWithValue("@name", user.Name);
                _command.Parameters.AddWithValue("@password", PasswordHasher.CreateSHA256(user.Password));
                _command.Connection = Connection;
                await _command.ExecuteReaderAsync();
            }
            catch (SqliteException ex)
            {
                Log.Error($"Error wile adding new user to Users table name: {user.Name} password: {user.Password} login: {user.Login}\n {ex.Message}");

            }
            catch (Exception ex)
            {
                
                Log.Error($"Error wile adding new user to Users table name: {user.Name}  password:  {user.Password} login: {user.Login}\n {ex.ToString()}");
            }
        }
        public async Task<User?> GetUser(string login)
        {
            try
            {
                SqliteCommand _command = new SqliteCommand();
                _command.CommandText = "SELECT login, name, password, id FROM Users WHERE login=@login";
                _command.Parameters.AddWithValue("@login", login);
                _command.Connection = Connection;
                SqliteDataReader reader = await _command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    reader.Read();
                    string db_login = (string)reader.GetValue(0);
                    string db_name = (string)reader.GetValue(1);
                    string db_password = (string)reader.GetValue(2);
                    Int64 db_id = (Int64)reader.GetValue(3);

                    Log.Debug($"{db_login} \t {db_name} \t {db_password} \t {db_id}");
                    return new User(name:db_name, login:db_login, password:db_password,id: db_id);

                }
                else
                {
                    Log.Warning($"DatabaseSqlite.GetUser reader.HasRows return False");
                    
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Error wile selecting User from table login: {login}\n{ex.ToString()}");

            }
            return null;

        }
        #endregion

    }
}
