using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using DBAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IADOEmbossingRepository: IADOCockroachDBRepository<Embossing>
    {
        Embossing GetEmbossingByCardNumber(string cardnumber);
        void Insert(Embossing embossing, DBAdapter.IDataBaseCommand dataBaseCommand);

        Embossing GetEmbossingByCardNumber(string cardnumber, string ccregion, IDataBaseCommand dataBaseCommand);
    }
}
