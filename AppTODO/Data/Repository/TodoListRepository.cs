using Dapper;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class TodoListRepository : ITodoListRepository
    {
        public readonly ConnectionStrings _connectionString;

        public TodoListRepository(ConnectionStrings connectionStrings)
        {
            this._connectionString = connectionStrings;
        }
        DynamicParameters parameters = new DynamicParameters();
        public int Create(ToDoViewModel toDoViewModel)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value))
            {
                var procName = "SP_InsertTodo";
                parameters.Add("@paramTodoName", toDoViewModel.ToDoName);
                parameters.Add("@paramUser", toDoViewModel.User);
                var todos = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return todos;
            }
        }
        public int Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value))
            {
                var procName = "SP_DeleteTodo";
                parameters.Add("@paramId", id);
                var todos = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return todos;
            }
        }

        public async Task<IEnumerable<ToDoViewModel>> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value))
            {
                var procName = "SP_GetAllTodo";
                var todos = await connection.QueryAsync<ToDoViewModel>(procName, commandType: CommandType.StoredProcedure);
                return todos;
            }
        }
        public async Task<IEnumerable<ToDoList>> Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value))
            {
                var procName = "SP_GetIdTodo";
                parameters.Add("@paramId", id);
                var todos = await connection.QueryAsync<ToDoList>(procName, parameters, commandType: CommandType.StoredProcedure);
                return todos;
            }
        }

        public int Update(int id, ToDoViewModel toDoViewModel)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value))
            {
                var procName = "SP_UpdateTodo";
                parameters.Add("@paramId", id);
                parameters.Add("@paramToDoName", toDoViewModel.ToDoName);
                parameters.Add("@paramStatus", toDoViewModel.Status);
                var todos = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return todos;
            }
        }
        public int UpdateStatus(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value))
            {
                var procName = "SP_UpdateToDoCompleted";
                parameters.Add("@paramId", id);
                var todos = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return todos;
            }
        }

        public async Task<ResponseTodo> Paging(string username, int status, string keyword, int pageSize, int pageNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value))
            {
                var procName = "SP_FilterDataByUser";
                parameters.Add("@paramUser", username);
                parameters.Add("@paramStatus", status);
                parameters.Add("@paramKeyword", keyword);
                parameters.Add("@paramPageSize", pageSize);
                parameters.Add("@paramPageNumber", pageNumber);
                parameters.Add("@length", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@filterlength", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var result = new ResponseTodo
                {
                    data = await connection.QueryAsync<ToDoList>(procName, parameters, commandType: CommandType.StoredProcedure)
                };
                int filterlength = parameters.Get<int>("@filterlength");
                result.filterlength = filterlength;
                int length = parameters.Get<int>("@length");
                result.length = length;
                return result;
            }
        }
    }
}
