using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interface
{
    public interface IItemRepository
    {
        IEnumerable<ItemViewModel> Get();
        Task<IEnumerable<ItemViewModel>> Get(int id);
        Task<ResponseItem> Paging(string keyword, int pageSize, int pageNumber);
        int Create(ItemViewModel itemViewModel);
        int Update(int id, ItemViewModel itemViewModel);
        int Delete(int id);
    }
}
