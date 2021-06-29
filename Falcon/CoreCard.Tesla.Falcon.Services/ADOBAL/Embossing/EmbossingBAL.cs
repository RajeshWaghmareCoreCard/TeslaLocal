using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
//using CockroachDb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCard.Tesla.Utilities;
using System.Data.SqlClient;
using CoreCard.Tesla.Falcon.ADORepository;
using DBAdapter;
using CoreCard.Tesla.Utilities;

namespace CoreCard.Tesla.Falcon.Services
{
    public class EmbossingBAL : /*BaseBAL<Embossing>, */IEmbossingBAL
    {
        //private readonly IEmbossingRepository _embossingRepository;
        private readonly IADOEmbossingRepository _iADOEmbossingRepository;
        public EmbossingBAL(/*IEmbossingRepository embossingRepository,*/ IADOEmbossingRepository iADOEmbossingRepository)// : base(embossingRepository)
        {
            //_embossingRepository = embossingRepository;
            _iADOEmbossingRepository = iADOEmbossingRepository;
        }

        //public async Task<Embossing> AddEmbossingAsync(Guid accountId)
        //{
        //    //            ◦ Generate EmbossingId
        //    //◦ Set CardType as 0
        //    //◦ Set AccountId to accounted of the account records
        //    //◦ Generate 16 digit CardNumber, encrypt and store

        //    Embossing embossing = new Embossing();
        //    //embossing.embossingid = Guid.NewGuid();
        //    embossing.accountid = accountId;
        //    embossing.cardtype = 0;
        //    embossing.cardnumber = AccountNoGenerator.RandomCardNumber();
        //    return await _embossingRepository.AddAsync(embossing);

        //}

        public Embossing GetEmbossingByCardNumber(string cardnumber)
        {
            return _iADOEmbossingRepository.GetEmbossingByCardNumber(cardnumber);
        }

        public Embossing GetEmbossingByCardNumber(string cardnumber, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            return _iADOEmbossingRepository.GetEmbossingByCardNumber(cardnumber, dataBaseCommand);
        }
        public void Insert(Guid accountid, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            Embossing embossing = new Embossing();
            //embossing.embossingid = Guid.NewGuid();
            embossing.accountid = accountid;
            embossing.cardtype = 0;
            embossing.cardnumber = AccountNoGenerator.RandomCardNumber();
            _iADOEmbossingRepository.Insert(embossing, dataBaseCommand);
        }
    }
}
