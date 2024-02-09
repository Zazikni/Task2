

using Microsoft.Data.Sqlite;
using System;
using System.IO;
using Serilog;
using AvaloniaUI.Models.Users;

namespace AvaloniaUI.Models.Database
{


    internal class DatabaseSqlite
    {
        private string _connectionString;
        private SqliteConnection _connection;
        private string _database_path;
        
        public string ConnectionString { get { return _connectionString; } }
        public SqliteConnection Connection { get { return _connection; } }
        public string DatabsePath { get; }
        public DatabaseSqlite(
            string DataSource = "project.db",
            SqliteOpenMode Mode = SqliteOpenMode.ReadWriteCreate,
            SqliteCacheMode Cache = SqliteCacheMode.Default,
            string? Password = null,
            bool? ForeignKeys = null
            )
        {
            //string _connectionString = $"DataSource={DataSource};Mode={Mode};Cache={Cache};{(Password != null ? "Password=" + Password + ";" : "")}{(ForeignKeys != null ? "ForeignKeys=" + ForeignKeys + ";" : "")}{(RecursiveTriggers != null ? "RecursiveTriggers=" + RecursiveTriggers + ";" : "")}";
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
            if (!File.Exists(DatabsePath))
            {
                _init_db();
                AddUser(name: "NIKITA", login: "ZAZIK", password: "KJKNKNKJw4cw3c4wr");
            }




        }
        private void _init_db()
        {

            SqliteCommand _command = new SqliteCommand();
            _command.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, login TEXT NOT NULL UNIQUE, name TEXT NOT NULL, password TEXT NOT NULL)";
            _command.Connection = Connection;
            _command.ExecuteNonQuery();

        }
        public void AddUser( string name, string password, string login)
        {
            try
            {
                SqliteCommand _command = new SqliteCommand();
                _command.CommandText = "INSERT INTO Users (login, name, password) VALUES (@login, @name, @password)";
                _command.Parameters.AddWithValue("@login", login);
                _command.Parameters.AddWithValue("@name", name);
                _command.Parameters.AddWithValue("@password", password);
                _command.Connection = Connection;
                _command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Error($"Error wile adding new user to Users table name: {name} password: {password} login: {login}");
            }


        }


        }
}
