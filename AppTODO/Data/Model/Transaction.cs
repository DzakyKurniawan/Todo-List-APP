using Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public class Transaction : BaseModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public Item Item { get; set; }
    }
}
