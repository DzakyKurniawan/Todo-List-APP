using Data.Base;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Model
{
    public class ToDoList : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string TodoName { get; set; }
        public int Status { get; set; }
        public User User { get; set; }
        public ToDoList() { }
        
    }
}
