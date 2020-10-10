using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service.Interface
{
    public interface ITodoListService
    {
        Task<IEnumerable<ToDoViewModel>> Get();
        Task<IEnumerable<ToDoList>> Get(int id);
        Task<ResponseTodo> Paging(string username, int status, string keyword, int pageSize, int pageNumber);
        int Create(ToDoViewModel toDoViewModel);
        int Update(int id, ToDoViewModel toDoViewModel);
        int UpdateStatus(int id);
        int Delete(int id);
    }
}
