using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class ResponseTransaction
    {
        public int Length { get; set; }
        public IEnumerable<TransactionViewModel> Data { get; set; }
        public int Filterlength { get; set; }
    }
}
