using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class SupplierViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset JoinDate { get; set; }
    }
}
