using Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class ResponseTodo
    {
        public int length { get; set; }
        public IEnumerable<ToDoList> data {get;set;}
        public int filterlength { get; set; }
    }
}
