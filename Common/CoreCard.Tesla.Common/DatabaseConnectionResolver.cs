using Npgsql;

namespace CoreCard.Tesla.Common
{
    public interface IDatabaseConnectionResolver
    {
        NpgsqlConnection GetNpgsqlConnection();
    }

    public class DatabaseConnectionResolver : IDatabaseConnectionResolver
    {
        private readonly string connectionString;

        public DatabaseConnectionResolver(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public NpgsqlConnection GetNpgsqlConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}