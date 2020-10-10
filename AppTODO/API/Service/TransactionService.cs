using API.Service.Interface;
using Data.Repository.Interface;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public int Create(TransactionViewModel transactionViewModel)
        {
            return _transactionRepository.Create(transactionViewModel);
        }

        public int Delete(int id)
        {
            return _transactionRepository.Delete(id);
        }

        public Task<IEnumerable<TransactionViewModel>> Get()
        {
            return _transactionRepository.Get();
        }

        public Task<ResponseTransaction> Paging(string keyword, int pageSize, int pageNumber)
        {
            return _transactionRepository.Paging(keyword, pageSize, pageNumber);
        }
    }
}
