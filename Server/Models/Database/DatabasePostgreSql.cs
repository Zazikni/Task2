using Serilog;
using System.Configuration;
using Models.Users;
using Npgsql;
using Models.Security;


namespace Models.Database
{
    /// <summary>
    /// Класс реализует взаимодействие с базой данных.
    /// Не имеет конструктора.
    /// Получить объект можно через свойство DatabasePostgreSql.Instance.
    /// </summary>
    internal class DatabasePostgreSql : IDatabase
    {
        #region fields
        string _connectionString;
        NpgsqlDataSource _dataSource;
        private static DatabasePostgreSql? _instance = null;
        public static DatabasePostgreSql Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabasePostgreSql();
                }
                return _instance;
            }
        }
        #endregion

        #region constructors
        private DatabasePostgreSql()
            
            
        {

            NpgsqlConnectionStringBuilder _csb = new NpgsqlConnectionStringBuilder();
            _csb.Host = ConfigurationManager.AppSettings["PostgreHost"];
            _csb.Database = ConfigurationManager.AppSettings["PostgreDatabase"];
            _csb.Username = ConfigurationManager.AppSettings["PostgreUsername"];
            _csb.Password = ConfigurationManager.AppSettings["PostgrePassword"];
            _csb.Port = Convert.ToInt32(ConfigurationManager.AppSettings["PostgrePort"]);
            _connectionString = _csb.ToString();
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
            _dataSource = dataSourceBuilder.Build();


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

            var w = GetUser("zazik");
            await w;


        }
        /// <summary>
        /// Метод для добавления нового пользователя в базу данных.
        /// </summary>
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
                Log.Debug($"Add new user to database login: {user.Login} \t name: {user.Name}");


            }
            catch (NpgsqlException ex)
            {
                Log.Error($"Error when adding a new user to Users table name: {user.Name} password: {user.Password} login: {user.Login}\n {ex.Message}");

            }
            catch (Exception ex)
            {

                Log.Error($"Error when adding a new user to Users table name: {user.Name}  password:  {user.Password} login: {user.Login}\n {ex.ToString()}");
            }
            finally
            {
                await connection.DisposeAsync();
            }
        }
        /// <summary>
        /// Метод для извлечения данных о пользователе из базы данных.
        /// </summary>

        public async Task<User?> GetUser(string login)
        {
            
            var connection = await _dataSource.OpenConnectionAsync();
            try
            {
                NpgsqlCommand _command = new NpgsqlCommand();
                _command.CommandText = "SELECT login, name, password, id FROM Users WHERE login=@login";
                _command.Parameters.AddWithValue("@login", login);
                _command.Connection = connection;
                var reader = await _command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    reader.Read();
                    string db_login = (string)reader.GetValue(0);
                    string db_name = (string)reader.GetValue(1);
                    string db_password = (string)reader.GetValue(2);
                    Int64 db_id = Convert.ToInt64(reader.GetValue(3));

                    Log.Debug($"{db_login} \t {db_name} \t {db_password} \t {db_id}");
                    return new User(name: db_name, login: db_login, password: db_password, id: db_id);

                }
                else
                {
                    Log.Warning($"DatabasePostgreSql.GetUser reader.HasRows return False");

                }


            }
            catch (NpgsqlException ex)
            {
                Log.Error($"Error wile selecting User from table login: {login}\n{ex.ToString()}");

            }
            catch (Exception ex)
            {

                Log.Error($"Error wile selecting User from table login: {login}\n{ex.ToString()}");
            }
            finally
            {
                await connection.DisposeAsync();
            }
            return null;

        }
        #endregion

    }
}

