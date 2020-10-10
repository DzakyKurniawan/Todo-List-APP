using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interface
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionViewModel>> Get();
        Task<ResponseTransaction> Paging(string keyword, int pageSize, int pageNumber);
        int Create(TransactionViewModel transactionViewModel);
        int Delete(int id);
    }
}
