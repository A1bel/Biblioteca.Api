using Microsoft.Data.SqlClient;

namespace Biblioteca.Api.Database
{
    public class DatabaseAccess
    {
        private readonly string _connectionString;

        public DatabaseAccess(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("Biblioteca");
        }

        public SqlConnection OpenConnection()
        {
            SqlConnection connection =
                new SqlConnection(_connectionString);

            connection.Open();

            return connection;
        }
    }
}
