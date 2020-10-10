    using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using API.Service.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private ITodoListService _todoListService;

        public ToDoController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        [HttpGet]
        public Task<IEnumerable<ToDoViewModel>> Get()
        {
            return _todoListService.Get();
        }

        [HttpGet("paging")]
        public async Task<ActionResult<ResponseTodo>> Paging(string username, int status, string keyword, int pageSize, int pageNumber)
        {
            if (keyword == null)
            {
                keyword = "";
            }
            var data = await _todoListService.Paging(username, status, keyword, pageSize, pageNumber);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<ToDoList>> Get(int id)
        {
            return await _todoListService.Get(id);
        }

        [HttpPost]
        public IActionResult Create(ToDoViewModel toDoViewModel)
        {
            var data = _todoListService.Create(toDoViewModel);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ToDoViewModel toDoViewModel)
        {
            var data = _todoListService.Update(id, toDoViewModel);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpDelete]
        [Route("status/{id}")]
        public IActionResult UpdateStatus(int id)
        {
            var data = _todoListService.UpdateStatus(id);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var data = _todoListService.Delete(id);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }
    }
}