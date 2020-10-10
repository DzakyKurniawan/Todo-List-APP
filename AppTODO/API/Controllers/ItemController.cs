using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Service.Interface;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public IEnumerable<ItemViewModel> Get()
        {
            return _itemService.Get();
        }

        [HttpGet("paging")]
        public async Task<ActionResult<ResponseItem>> Paging(string keyword, int pageSize, int pageNumber)
        {
            if (keyword == null)
            {
                keyword = "";
            }
            var data = await _itemService.Paging(keyword, pageSize, pageNumber);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<ItemViewModel>> Getby(int id)
        {
            return await _itemService.Get(id);
        }

        [HttpPost]
        public IActionResult Create(ItemViewModel itemViewModel)
        {
            var data = _itemService.Create(itemViewModel);
            if(data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ItemViewModel itemViewModel)
        {
            var data = _itemService.Update(id, itemViewModel);
            if(data > 0){
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var data = _itemService.Delete(id);
            if(data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        } 
    }
}