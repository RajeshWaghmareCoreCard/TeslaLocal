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
using Microsoft.Extensions.Configuration;

namespace CoreCard.Tesla.Falcon.Services
{
    public class EmbossingBAL : /*BaseBAL<Embossing>, */IEmbossingBAL
    {
        //private readonly IEmbossingRepository _embossingRepository;
        private readonly IADOEmbossingRepository _iADOEmbossingRepository;
        protected IConfiguration _configuration;
        public EmbossingBAL(/*IEmbossingRepository embossingRepository,*/ IADOEmbossingRepository iADOEmbossingRepository, IConfiguration configuration)// : base(embossingRepository)
        {
            //_embossingRepository = embossingRepository;
            _iADOEmbossingRepository = iADOEmbossingRepository;
            _configuration = configuration;
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
            //string [] binNumbers = _configuration.GetSection("BinNumber").Value.Split(',');
            long binnumber = Convert.ToInt64(_configuration.GetSection("BinNumber").Value);
            embossing.binnumber = binnumber;
            embossing.accountid = accountid;
            embossing.cardtype = 0;
            embossing.ccregion = Convert.ToString(_configuration.GetSection("ccregion").Value);
            embossing.cardnumber = AccountNoGenerator.RandomCardNumber();
            _iADOEmbossingRepository.Insert(embossing, dataBaseCommand);
        }
    }
}
