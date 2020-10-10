using Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class ResponseItem
    {
        public int Length { get; set; }
        public IEnumerable<ItemViewModel> Data { get; set; }
        public int Filterlength { get; set; }
    }
}
