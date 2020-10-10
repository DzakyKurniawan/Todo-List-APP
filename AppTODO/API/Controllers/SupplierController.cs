using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Service.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [HttpGet]
        public IEnumerable<Supplier> Get()
        {
            return _supplierService.Get();
        }

        [HttpGet("{id}")]
        public Task<IEnumerable<Supplier>> Get(int id)
        {
            return _supplierService.Get(id);
        }

        [HttpPost]
        public IActionResult Create(SupplierViewModel supplierViewModel)
        {
            var data = _supplierService.Create(supplierViewModel);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, SupplierViewModel supplierViewModel)
        {
            var data = _supplierService.Update(id, supplierViewModel);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var data = _supplierService.Delete(id);
            if (data > 0)
            {
                return Ok(data);
            }
            return BadRequest("Bad Request");
        }
    }
}