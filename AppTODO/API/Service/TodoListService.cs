using API.Service.Interface;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service
{
    public class TodoListService : ITodoListService
    {
        private ITodoListRepository _todoListRepository;
        public TodoListService(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }
        public int Create(ToDoViewModel toDoViewModel)
        {
            return _todoListRepository.Create(toDoViewModel);
        }

        public int Delete(int id)
        {
            return _todoListRepository.Delete(id);
        }

        public async Task<IEnumerable<ToDoViewModel>> Get()
        {
            return await _todoListRepository.Get();
        }

        public async Task<IEnumerable<ToDoList>> Get(int id)
        {
            return await _todoListRepository.Get(id);
        }

        public Task<ResponseTodo> Paging(string username, int status, string keyword, int pageSize, int pageNumber)
        {
            return _todoListRepository.Paging(username, status, keyword, pageSize, pageNumber);
        }

        public int Update(int id, ToDoViewModel toDoViewModel)
        {
            return _todoListRepository.Update(id, toDoViewModel);
        }

        public int UpdateStatus(int id)
        {
            return _todoListRepository.UpdateStatus(id);
        }
    }
}
