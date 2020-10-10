using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public int Item { get; set; }
        public string ItemName { get; set; }

    }
}
