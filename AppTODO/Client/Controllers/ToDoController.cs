using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataTables.AspNet.Core;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Client.Controllers
{
    public class ToDoController : Controller
    {
        readonly HttpClient client = new HttpClient();
        SmtpClient smtpClient;
        NetworkCredential networkCredential;
        MailMessage mailMessage;

        public ToDoController()
        {
            client.BaseAddress = new Uri("https://localhost:44324/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
        //[Authorize(Roles ="Client")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Get(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            HttpResponseMessage response = await client.GetAsync("ToDo");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<ToDoViewModel>>();
                var item = data.FirstOrDefault(s => s.Id == id);
                var json = JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                return Json(json);
            }
            return Json("Internal Server Error");
        }
        public async Task RefreshToken(TokenViewModel tokenViewModel)
        {
            var myContent = JsonConvert.SerializeObject(tokenViewModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result2 = client.PostAsync("Account/refresh", byteContent).Result;

            if (result2.StatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                var tes = await result2.Content.ReadAsAsync<Token>();
                var token2 = "Bearer " + tes.AccessToken;
                var exptoken2 = tes.ExpireToken;
                var reftoken2 = tes.RefreshToken;
                var expreftoken2 = tes.ExpireRefreshToken;
                HttpContext.Session.SetString("JWToken", token2);
                HttpContext.Session.SetString("ExpToken", exptoken2.ToString());
                HttpContext.Session.SetString("RefreshToken", reftoken2);
                HttpContext.Session.SetString("ExpRefToken", expreftoken2.ToString());
            }
        }

        public async Task<ResponseTodo> Paging(int status, string keyword, int pageSize, int pageNumber)
        {
            TokenViewModel tokenViewModel = new TokenViewModel();
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var username = HttpContext.Session.GetString("Username");
            var exptoken = Convert.ToInt64(HttpContext.Session.GetString("ExpToken"));
            var reftoken = HttpContext.Session.GetString("RefreshToken");
            var expreftoken = Convert.ToInt64(HttpContext.Session.GetString("ExpRefToken"));

            if (exptoken < DateTime.UtcNow.Ticks && expreftoken > DateTime.UtcNow.Ticks)
            {
                await RefreshToken(tokenViewModel);
            }
            else if (expreftoken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            try
            {
                var url = "ToDo/paging?username=" + username + "&status=" + status + "&keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber;
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var d = await response.Content.ReadAsAsync<ResponseTodo>();
                    return d;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public JsonResult Save(ToDoViewModel toDoViewModel)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            toDoViewModel.User = HttpContext.Session.GetString("Username");
            var myContent = JsonConvert.SerializeObject(toDoViewModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (toDoViewModel.Id == 0)
            {
                var result = client.PostAsync("ToDo", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("ToDo/" + toDoViewModel.Id, byteContent).Result;
                return Json(result);
            }
        }

        public async Task<JsonResult> UpdateStatus(ToDoViewModel toDoViewModel)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var username = HttpContext.Session.GetString("Username");
            string todoname = null;
            string status = "completed";
            var result2 = client.DeleteAsync("ToDo/status/" + toDoViewModel.Id).Result;


            HttpResponseMessage response = await client.GetAsync("ToDo");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<ToDoViewModel>>();
                var item = data.FirstOrDefault(s => s.Id == toDoViewModel.Id);
                var json = JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                todoname = item.ToDoName;             
            }

            networkCredential = new NetworkCredential("viaoktabela@gmail.com", "bela101010");
            smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = Convert.ToInt32("587"),
                EnableSsl = true,
                Credentials = networkCredential
            };
            mailMessage = new MailMessage { From = new MailAddress("viaoktabela@gmail.com", "Status ToDoList", Encoding.UTF8) };
            mailMessage.To.Add(new MailAddress(username));
            mailMessage.Subject = "Your ToDo List";
            mailMessage.Body = "ToDo List: " + todoname + " has been " + status;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.Low;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            string userstate = "Sending";
            smtpClient.SendAsync(mailMessage, userstate);

            return Json(result2);
        }
        public JsonResult Delete(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var result = client.DeleteAsync("ToDo/delete/" + id).Result;
            return Json(result);
        }

        [HttpGet("ToDo/PageData/{status}")]
        public IActionResult PageData(IDataTablesRequest request, int status)
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
            var dataPage = Paging(status, keyword, pageSize, pageNumber).Result;
            //var dataPage = GetFilterData(pageSize, pageNumber, status, keyword).Result;
            var filteredData = dataPage.filterlength;
            // Response creation. To create your response you need to reference your request, to avoid

            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, dataPage.length, filteredData, dataPage.data);

            // Easier way is to return a new 'DataTablesJsonResult', which will automatically convert your

            // response to a json-compatible content, so DataTables can read it when received.
            return new DataTablesJsonResult(response, true);

        }
    }
}