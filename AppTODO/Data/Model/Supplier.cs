using Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public class Supplier : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset JoinDate { get; set; }
    }
}
