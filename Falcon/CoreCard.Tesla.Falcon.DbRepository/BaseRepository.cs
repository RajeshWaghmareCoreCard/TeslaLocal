using CoreCard.Tesla.Common;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public abstract class BaseRepository
    {
        protected readonly IDatabaseConnectionResolver databaseConnection;
        public BaseRepository(IDatabaseConnectionResolver databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }
    }
}