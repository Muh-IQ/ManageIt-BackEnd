namespace Data_Access_Tier
{
    public class ConnectionProvider
    {
        private static ConnectionProvider _instance;
        private static readonly object _lock = new object();


        private  string _connectionString = string.Empty;
        private ConnectionProvider()
        {
            _connectionString = _GetConnection();
        }

        public static ConnectionProvider Instance { get

            { 
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new ConnectionProvider();
                    }

                }
                return _instance;
            } 
        }
        public string ConnectionString => _connectionString;
       

        private string _GetConnection()
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("connection with DB is not found.");

            return connectionString;
        }

    }
}
