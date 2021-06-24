using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
//using CockroachDb.Repository;
using DBAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public class TransactionBAL:ITransactionBAL
    {
        //private readonly ITransactionRepository _transactionRepository;
        private readonly IADOTransactionRepository _iADOTransactionRepository;

        public TransactionBAL( IADOTransactionRepository iADOTransactionRepository) //: base(transactionRepository)
        {
            //_transactionRepository = transactionRepository;
            _iADOTransactionRepository = iADOTransactionRepository;
        }

        public Transaction AddTransactionADO(Transaction transaction)
        {
            return _iADOTransactionRepository.Add(transaction);
        }

        public Guid AddTransactionADO(Transaction transaction, IDataBaseCommand dbCommand)
        {
            return _iADOTransactionRepository.Add(transaction,dbCommand);
        }

        //public Task<Transaction> AddTransactionAsync(TransactionAddDTO transactioDTO)
        //{
        //    Transaction transaction = TransactionAddDTO.MapToTransaction(transactioDTO);
        //    return _transactionRepository.AddAsync(transaction);
        //}
    }
}
