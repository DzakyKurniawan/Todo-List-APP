using API.Service.Interface;
using Data.Repository.Interface;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service
{
    public class ItemService : IItemService
    {
        private IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public int Create(ItemViewModel itemViewModel)
        {
            return _itemRepository.Create(itemViewModel);
        }

        public int Delete(int id)
        {
            return _itemRepository.Delete(id);
        }

        public Task<IEnumerable<ItemViewModel>> Get(int id)
        {
            return _itemRepository.Get(id);
        }

        public IEnumerable<ItemViewModel> Get()
        {
            return _itemRepository.Get();
        }

        public Task<ResponseItem> Paging(string keyword, int pageSize, int pageNumber)
        {
            return _itemRepository.Paging(keyword, pageSize, pageNumber);
        }

        public int Update(int id, ItemViewModel itemViewModel)
        {
            return _itemRepository.Update(id, itemViewModel);
        }
    }
}
