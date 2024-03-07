using Models.Security;
using Models.Users;
using Npgsql;
using Serilog;
using ConfigurationManager = Server.Configuration.ConfigurationManager;

namespace Models.Database
{
    /// <summary>
    /// Класс реализует взаимодействие с базой данных.
    /// Не имеет конструктора.
    /// Получить объект можно через свойство DatabasePostgreSql.Instance.
    /// </summary>
    internal class DatabasePostgreSql : IDatabase
    {
        #region Fields

        private string _connectionString;
        private NpgsqlDataSource _dataSource;
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

        #endregion Fields

        #region Constructors

        private DatabasePostgreSql()

        {
            NpgsqlConnectionStringBuilder _csb = new NpgsqlConnectionStringBuilder();

            _csb.Host = ConfigurationManager.Instance.RootSettings.Database.PostgreHost;
            _csb.Database = ConfigurationManager.Instance.RootSettings.Database.PostgreDatabaseName;
            _csb.Username = ConfigurationManager.Instance.RootSettings.Database.PostgreUsername;
            _csb.Password = ConfigurationManager.Instance.RootSettings.Database.PostgrePassword;
            _csb.Port = ConfigurationManager.Instance.RootSettings.Database.PostgrePort;

            _connectionString = _csb.ToString();
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
            _dataSource = dataSourceBuilder.Build();
            _CheckConnection();
        }

        #endregion Constructors

        #region Methods

        private async Task _init()
        {
            var x = AddUser(new NewUser(name: "Nikita", login: "zazik", password: "K2342^%&KNKNKJw4cw3c4wr"));
            var y = AddUser(new NewUser(name: "Oleg", login: "katsd", password: "712876asKasdsassdr"));
            var z = AddUser(new NewUser(name: "Billy", login: "billy212", password: "45466&%#dt56asdasd."));
            await x;
            await y;
            await z;

            var w = GetUser("zazik");
            await w;
        }

        private void _CheckConnection()
        {
            Log.Information("Проверка соеденения с базой данных.");
            try
            {
                using (var connection = _dataSource.OpenConnection())
                {
                    // Проверяем состояние соединения
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        Log.Information("Соединение с базой данных успешно установлено.");
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Log.Error("Ошибка при подключении к базе данных: " + ex.Message);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Log.Error("Произошла ошибка: " + ex.Message);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Метод для добавления нового пользователя в базу данных.
        /// </summary>
        public async Task AddUser(NewUser user)
        {
            Log.Debug($"Добавление нового пользователя в базу данных. Login: {user.Login}\tName: {user.Name}");

            NpgsqlConnection connection;
            try
            {
                connection = await GetConnectionAsync();

            }
            catch (DatabaseConnectionError ex) { Log.Error(ex.Message); throw new DatabaseConnectionError(ex.Message); }

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
                Log.Error($"Ошибка при добавлении пользователя в базу данных Name: {user.Name} Password: {user.Password} Login: {user.Login}\n {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error($"Ошибка при добавлении пользователя в базу данных Name: {user.Name} Password: {user.Password} Login: {user.Login}\n {ex.Message}");
                throw ex;
            }
            finally
            {
                await connection.DisposeAsync();
            }
        }
        private async Task<NpgsqlConnection> GetConnectionAsync()
        {
            try
            {
                NpgsqlConnection connection = await _dataSource.OpenConnectionAsync();
                return connection;

            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseConnectionError("Ошибка при подключении к базе данных: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionError("Произошла ошибка: " + ex.Message);
            }

        }

        /// <summary>
        /// Метод для извлечения данных о пользователе из базы данных.
        /// </summary>

        public async Task<User?> GetUser(string login)
        {
            NpgsqlConnection connection;
            try
            {
                connection = await GetConnectionAsync();

            }
            catch (DatabaseConnectionError ex) { Log.Error(ex.Message); throw new DatabaseConnectionError(ex.Message); }
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

                    Log.Debug($"Информация из базы данных {db_login}\t{db_name}\t{db_password}\t{db_id}");
                    return new User(name: db_name, login: db_login, password: db_password, id: db_id);
                }
                else
                {
                    Log.Warning($"База данных не вернула никаких данных.");
                }
            }
            catch (NpgsqlException ex)
            {
                Log.Error($"Ошибка при получении пользователя из базы данных Login: {login}\n{ex.ToString()}");
            }
            catch (IOException ex)
            {
                Log.Error($"Ошибка соеденения с базой данных");
            }
            catch (Exception ex)
            {
                Log.Error($"Ошибка при получении пользователя из базы данных Login: {login}\n{ex.ToString()}");
            }

            finally
            {
                await connection.DisposeAsync();
            }
            return null;
        }

        #endregion Methods
    }
}