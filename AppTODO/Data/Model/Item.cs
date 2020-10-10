using Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public class Item : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public Supplier Supplier { get; set; }
    }
}
