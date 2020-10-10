using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Client.Controllers
{
    public class TransactionController : Controller
    {
        readonly HttpClient client = new HttpClient();
        public TransactionController()
        {
            client.BaseAddress = new Uri("https://localhost:44324/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        [Authorize(Roles= "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Get()
        {
            HttpResponseMessage response = await client.GetAsync("Transaction");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<Transaction>>();
                var json = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(json);
            }
            return Json("Internal Server Error");
        }

        public JsonResult Save(TransactionViewModel transactionViewModel)
        {
            var myContent = JsonConvert.SerializeObject(transactionViewModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (transactionViewModel.Id == 0)
            {
                var result = client.PostAsync("Transaction", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Transaction/" + transactionViewModel.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult Delete(int id)
        {
            var result = client.DeleteAsync("Transaction/" + id).Result;
            return Json(result);
        }

        public async Task<ResponseTransaction> Paging(string keyword, int pageSize, int pageNumber)
        {
            var url = "Transaction/paging?keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var d = await response.Content.ReadAsAsync<ResponseTransaction>();
                return d;
            }
            return null;
        }

        [HttpGet("Transaction/PageData")]
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

        public async Task<IActionResult> Excel()
        {
            var comlumHeadrs = new string[]
            {
                "Item",
                "Quantity",
                "Total Price",
                "Transaction Date"
            };
            byte[] result;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Current Transaction");
                using (var cells = worksheet.Cells[1, 1, 1, 4])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Style.Font.Size = 14;
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    //border the cell
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //set background color for each sell
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 243, 214));
                }
                var j = 2;
                HttpResponseMessage response = await client.GetAsync("Transaction");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<IList<TransactionViewModel>>();
                    var count = data.Count();
                    foreach (var transaction in data)
                    {
                        for (int col = 1; col <= comlumHeadrs.Length; col++)
                        {
                            worksheet.Cells[j, col].Style.Font.Size = 12;
                            //worksheet.Cells[row, col].Style.Font.Bold = true; 
                            worksheet.Cells[j, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        }
                        worksheet.Cells["A" + j].Value = transaction.ItemName;
                        worksheet.Cells["B" + j].Value = transaction.Quantity;
                        worksheet.Cells["C" + j].Value = String.Format(CultureInfo.CreateSpecificCulture("id-id"), "Rp. {0:N}",transaction.TotalPrice);
                        worksheet.Cells["D" + j].Value = transaction.TransactionDate.ToString("dd MMMM yyyy", new CultureInfo("id-ID"));

                        j++;
                    }
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Transaction.xlsx");
        }

        public async Task<IActionResult> CSV()
        {
            var comlumHeadrs = new string[]
            {
                "Item",
                "Quantity",
                "Total Price",
                "Transaction Date"
            };
            byte[] buffer;
            var transactioncsv = new StringBuilder();
            HttpResponseMessage response = await client.GetAsync("Transaction");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<TransactionViewModel>>();
                var transactionRecord = (from transaction in data
                                         select new object[]
                                         {
                                            $"{transaction.ItemName}",
                                            $"\"{transaction.Quantity}\"", //Escaping ","
                                            $"\"{transaction.TotalPrice.ToString("$#,0.00;($#,0.00)")}\"", //Escaping ","
                                            transaction.TransactionDate.ToString("MM/dd/yyyy"),
                                         }).ToList();


                transactionRecord.ForEach(line =>
                {
                    transactioncsv.AppendLine(string.Join(",", line));
                });
            }
            buffer = Encoding.ASCII.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{transactioncsv.ToString()}");
            return File(buffer, "text/csv", $"Transaction.csv");
        }
        public IActionResult Report()
        {
            TransactionReport transactionReport = new TransactionReport();
            byte[] abytes = transactionReport.PrepareReport();
            return File(abytes, "application/pdf", $"Transaction.pdf");
        }

        //public async Task<List<TransactionViewModel>> GetTransaction()
        //{
        //    List<TransactionViewModel> transactionViewModels = new List<TransactionViewModel>();
        //    TransactionViewModel transactionViewModel = new TransactionViewModel();
        //    HttpResponseMessage response = await client.GetAsync("Transaction");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = await response.Content.ReadAsAsync<IList<TransactionViewModel>>();
        //        var count = data.Count();
        //        for (int i = 1; i <= count; i++)
        //        {
        //            transactionViewModel = new TransactionViewModel
        //            {
        //                Id = i,
        //                ItemName = transactionViewModel.ItemName,
        //                Quantity = transactionViewModel.Quantity,
        //                TransactionDate = transactionViewModel.TransactionDate,
        //                TotalPrice = transactionViewModel.TotalPrice
        //            };
        //        }
        //    }
        //    return transactionViewModels;
        //}
    }
}
