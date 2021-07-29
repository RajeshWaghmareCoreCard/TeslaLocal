using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
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
