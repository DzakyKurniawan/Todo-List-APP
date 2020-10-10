using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class ItemController : Controller
    {
        readonly HttpClient client = new HttpClient();

        public ItemController()
        {
            client.BaseAddress = new Uri("https://localhost:44324/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Get()
        {
            var url = "Item/itemlist";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var d = await response.Content.ReadAsAsync<IList<ItemViewModel>>();
                return Ok(new { data = d });
            }
            return Json("Internal Server Error");
        }

        public JsonResult JsonList()
        {
            var client = new HttpClient();
            var responseTask = client.GetAsync("https://localhost:44324/API/Item");
            responseTask.Wait();

            var result = responseTask.Result;
            IEnumerable<ItemViewModel> items;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<ItemViewModel>>();
                readTask.Wait();
                items = readTask.Result;
            }
            else
            {
                items = Enumerable.Empty<ItemViewModel>();
                ModelState.AddModelError(string.Empty, "Server error");
            }
            return Json(items);
        }

        public async Task<JsonResult> Getby(int id)
        {
            HttpResponseMessage response = await client.GetAsync("Item");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<ItemViewModel>>();
                var item = data.FirstOrDefault(i => i.Id == id);
                var json = JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(json);
            }
            return Json("Internal Server Error");
        }

        public async Task<ResponseItem> Paging(string keyword, int pageSize, int pageNumber)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var url = "Item/paging?keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var d = await response.Content.ReadAsAsync<ResponseItem>();
                return d;
            }
            return null;
        }

        public JsonResult Save(ItemViewModel itemViewModel)
        {
            var myContent = JsonConvert.SerializeObject(itemViewModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if(itemViewModel.Id == 0)
            {
                var result = client.PostAsync("Item", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Item/" + itemViewModel.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult Delete(int id)
        {
            var result = client.DeleteAsync("Item/" + id).Result;
            return Json(result);
        }
        [HttpGet("Item/PageData")]
        public IActionResult PageData(IDataTablesRequest request)
        {
            // Nothing important here. Just creates some mock data.
            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length + 1;
            string keyword = string.Empty;
            if (request.Search.Value != null)
            {
                keyword = request.Search.Value;
            }

            //var data = Searching(status, keyword).Result;            
            // Global filtering.

            // Filter is being manually applied due to in-memmory (IEnumerable) data.

            // If you want something rather easier, check IEnumerableExtensions Sample.

            // Paging filtered data.

            // Paging is rather manual due to in-memmory (IEnumerable) data.
            var dataPage = Paging(keyword, pageSize, pageNumber).Result;
            //var dataPage = GetFilterData(pageSize, pageNumber, status, keyword).Result;
            var filteredData = dataPage.Filterlength;
            // Response creation. To create your response you need to reference your request, to avoid

            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, dataPage.Length, filteredData, dataPage.Data);

            // Easier way is to return a new 'DataTablesJsonResult', which will automatically convert your

            // response to a json-compatible content, so DataTables can read it when received.
            return new DataTablesJsonResult(response, true);

        }
        public IActionResult Error()
        {
            return View();
        }

    }
}