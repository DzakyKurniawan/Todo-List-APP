using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class ToDoViewModel
    {
        public int Id { get; set; }
        public string ToDoName { get; set; }
        public int Status { get; set; }
        public string User { get; set; }
    }
}
