using Data.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class TransactionReport
    {
        readonly HttpClient client = new HttpClient();
        public TransactionReport()
        {
            client.BaseAddress = new Uri("https://localhost:44324/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        #region Declaration
        readonly int _totalColumn = 5;
        Document _document;
        Font _fontStyle;
        readonly PdfPTable _pdfTable = new PdfPTable(5);
        PdfPCell _pdfPCell;
        readonly MemoryStream _memoryStream = new MemoryStream();
        #endregion
        public byte[] PrepareReport()
        {
            #region
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            _pdfTable.SetWidths(new float[] {20f,80f, 20f,50f,50f});
            #endregion

            this.ReportHeader();
            this.ReportBody();
            _pdfTable.HeaderRows = 2;
            _document.Add(_pdfTable);
            _document.Close();
            return _memoryStream.ToArray();

        }
        private void ReportHeader()
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _pdfPCell = new PdfPCell(new Phrase("To Do APP", _fontStyle))
            {
                Colspan = _totalColumn,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
                BackgroundColor = BaseColor.WHITE,
                ExtraParagraphSpace = 0
            };
            _pdfTable.AddCell(_pdfPCell);
            _pdfTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Transaction Report", _fontStyle))
            {
                Colspan = _totalColumn,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
                BackgroundColor = BaseColor.WHITE,
                ExtraParagraphSpace = 0
            };
            _pdfTable.AddCell(_pdfPCell);
            _pdfTable.CompleteRow();

        }

        private async void ReportBody()
        {
            #region Table Header
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("No Transaction", _fontStyle))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
            };
            _pdfTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Item Name", _fontStyle))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
            };
            _pdfTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Quantity", _fontStyle))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
            };
            _pdfTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Total Price", _fontStyle))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
            };
            _pdfTable.AddCell(_pdfPCell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Transaction Date", _fontStyle))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
            };
            _pdfTable.AddCell(_pdfPCell);
            _pdfTable.CompleteRow();
            #endregion

            #region TableBody
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            int serialNumber = 1;
            HttpResponseMessage response = await client.GetAsync("Transaction");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<TransactionViewModel>>();
                foreach (var transactionViewModel in data)
                {
                    _pdfPCell = new PdfPCell(new Phrase(serialNumber++.ToString(), _fontStyle))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.WHITE,
                    };
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(transactionViewModel.ItemName, _fontStyle))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.WHITE,
                    };
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(transactionViewModel.Quantity.ToString(), _fontStyle))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.WHITE,
                    };
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(String.Format(CultureInfo.CreateSpecificCulture("id-id"), "Rp. {0:N}",transactionViewModel.TotalPrice), _fontStyle))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.WHITE,
                    };
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(transactionViewModel.TransactionDate.ToString("dd MMMM yyyy", new CultureInfo("id-ID")), _fontStyle))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.WHITE,
                    };
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();
                }
            }
            #endregion
        }
    }
}
