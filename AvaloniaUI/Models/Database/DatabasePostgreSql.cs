using AvaloniaUI.Models.Users;
using System;
using Npgsql;
using System.Threading.Tasks;
using AvaloniaUI.Models.Security;
using Microsoft.Data.Sqlite;
using Serilog;
using Tmds.DBus.Protocol;


namespace AvaloniaUI.Models.Database
{
    internal class DatabasePostgreSql //: IDatabase
    {
        #region fields
        string _connectionString;
        NpgsqlDataSource _dataSource;
        #endregion
        #region constructors
        public DatabasePostgreSql(string host, string database, string username, string password, int port)
        {
            NpgsqlConnectionStringBuilder _csb = new NpgsqlConnectionStringBuilder();
            _csb.Host = host;
            _csb.Database = database;
            _csb.Username = username;
            _csb.Password = password;
            _csb.Port = port;
            _connectionString = _csb.ToString();
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
            _dataSource = dataSourceBuilder.Build();

            _init();


        }
        #endregion
        #region methods
        private async Task _init()
        {
            var x =  AddUser(new NewUser(name: "Nikita", login: "zazik", password: "K2342^%&KNKNKJw4cw3c4wr"));
            var y = AddUser(new NewUser(name: "Oleg", login: "katsd", password: "712876asKasdsassdr"));
            var z = AddUser(new NewUser(name: "Billy", login: "billy212", password: "45466&%#dt56asdasd."));
            await x;
            await y;
            await z;
        }
        public async Task AddUser(NewUser user)
        {
            var connection = await _dataSource.OpenConnectionAsync();
            try
            {
                NpgsqlCommand _command = new NpgsqlCommand();
                _command.CommandText = "INSERT INTO Users (login, name, password) VALUES (@login, @name, @password)";
                _command.Connection = connection;
                _command.Parameters.AddWithValue("@login", user.Login);
                _command.Parameters.AddWithValue("@name", user.Name);
                _command.Parameters.AddWithValue("@password", PasswordHasher.CreateSHA256(user.Password));
                await _command.ExecuteNonQueryAsync();


            }
            catch (NpgsqlException ex)
            {
                Log.Error($"Error wile adding new user to Users table name: {user.Name} password: {user.Password} login: {user.Login}\n {ex.Message}");

            }
            catch (Exception ex)
            {

                Log.Error($"Error wile adding new user to Users table name: {user.Name}  password:  {user.Password} login: {user.Login}\n {ex.ToString()}");
            }
            finally
            {
                connection.Close();
            }
        }

        public User? GetUser(string login)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
