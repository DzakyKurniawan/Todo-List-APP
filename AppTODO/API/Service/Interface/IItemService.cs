using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service.Interface
{
    public interface IItemService
    {
        IEnumerable<ItemViewModel> Get();
        Task<IEnumerable<ItemViewModel>> Get(int id);
        Task<ResponseItem> Paging(string keyword, int pageSize, int pageNumber);
        int Create(ItemViewModel itemViewModel);
        int Update(int id, ItemViewModel itemViewModel);
        int Delete(int id);
    }
}
