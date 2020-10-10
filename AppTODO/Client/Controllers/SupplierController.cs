using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class SupplierController : Controller
    {
        readonly HttpClient client = new HttpClient();

        public SupplierController()
        {
            client.BaseAddress = new Uri("https://localhost:44324/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Get()
        {
            HttpResponseMessage response = await client.GetAsync("Supplier");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<Supplier>>();
                var json = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(json);
            }
            return Json("Internal Server Error");
        }

        public async Task<JsonResult> Get(int id)
        {
            HttpResponseMessage response = await client.GetAsync("Supplier");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<Supplier>>();
                var supplier = data.FirstOrDefault(s => s.Id == id);
                var json = JsonConvert.SerializeObject(supplier, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(json);
            }
            return Json("Internal Server Error");
        }

        public JsonResult Save(SupplierViewModel supplierViewModel)
        {
            var myContent = JsonConvert.SerializeObject(supplierViewModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if(supplierViewModel.Id == 0)
            {
                var result = client.PostAsync("Supplier", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Supplier/" + supplierViewModel.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult Delete(int id)
        {
            var result = client.DeleteAsync("Supplier/" + id).Result;
            return Json(result);
        }

        public JsonResult JsonList()
        {
            IEnumerable<Supplier> suppliers = null;
            var client = new HttpClient();
            var responseTask = client.GetAsync("https://localhost:44324/API/Supplier");
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Supplier>>();
                readTask.Wait();
                suppliers = readTask.Result;
            }
            else
            {
                suppliers = Enumerable.Empty<Supplier>();
                ModelState.AddModelError(string.Empty, "Server error");
            }
            return Json(suppliers);
        }

    }
}