namespace LibraryManagementSystem.Connetcion
{
    public class Connection
    {
        private IConfiguration _configuration;
        public Connection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnection()
        {
            string connString =  _configuration.GetConnectionString("MyDbConnection");

            return connString;
        }


    }
}
