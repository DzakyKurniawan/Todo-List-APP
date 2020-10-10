using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service.Interface
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionViewModel>> Get();
        Task<ResponseTransaction> Paging(string keyword, int pageSize, int pageNumber);
        int Create(TransactionViewModel transactionViewModel);
        int Delete(int id);
    }
}
