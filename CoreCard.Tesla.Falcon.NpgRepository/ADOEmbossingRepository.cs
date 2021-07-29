using CoreCard.Tesla.Falcon.DataModels.Entity;

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
{
    public class ADOEmbossingRepository:BaseRepository,IADOEmbossingRepository
    {
        public ADOEmbossingRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {

        }

        public Embossing GetEmbossingByCardNumber(string cardnumber)
        {
            throw new NotImplementedException();
        }

        public void Insert(Embossing embossing)
        {
            throw new NotImplementedException();
        }
    }
}
