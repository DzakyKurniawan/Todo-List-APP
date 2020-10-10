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
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public Task<IEnumerable<TransactionViewModel>> Get()
        {
            return _transactionService.Get();
        }

        [HttpGet("paging")]
        public async Task<ActionResult<ResponseTransaction>> Paging(string keyword, int pageSize, int pageNumber)
        {
            if (keyword == null)
            {
                keyword = "";
            }
            var data = await _transactionService.Paging(keyword, pageSize, pageNumber);
            if (data != null)
            {
                return Ok(data);
            }
            return NotFound("Not Found");
        }

        [HttpPost]
        public IActionResult Create(TransactionViewModel transactionViewModel)
        {
            var data = _transactionService.Create(transactionViewModel);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var data = _transactionService.Delete(id);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }
    }
}