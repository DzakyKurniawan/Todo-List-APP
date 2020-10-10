using Dapper;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class ItemRepository : IItemRepository
    {
        public readonly ConnectionStrings _connectionStrings;
        readonly DynamicParameters parameters = new DynamicParameters();
        public ItemRepository(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }
        public int Create(ItemViewModel itemViewModel)
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_CreateItem";
            parameters.Add("@paramName", itemViewModel.Name);
            parameters.Add("@paramPrice", itemViewModel.Price);
            parameters.Add("@paramStock", itemViewModel.Stock);
            parameters.Add("@paramSupplier", itemViewModel.Supplier);
            var items = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            return items;
        }

        public int Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_DeleteItem";
            parameters.Add("@paramId", id);
            var items = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            return items;
        }

        public async Task<IEnumerable<ItemViewModel>> Get(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_GetIdItem";
            parameters.Add("@paramId", id);
            var items = await connection.QueryAsync<ItemViewModel>(procName, parameters, commandType: CommandType.StoredProcedure);
            return items;
        }

        public IEnumerable<ItemViewModel> Get()
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_GetAllItem";
            var items = connection.Query<ItemViewModel>(procName, parameters, commandType: CommandType.StoredProcedure);
            return items;
        }

        public async Task<ResponseItem> Paging(string keyword, int pageSize, int pageNumber)
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_FilterDataItem";
            parameters.Add("@paramKeyword", keyword);
            parameters.Add("@paramPageSize", pageSize);
            parameters.Add("@paramPageNumber", pageNumber);
            parameters.Add("@length", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@filterlength", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = new ResponseItem
            {
                Data = await connection.QueryAsync<ItemViewModel>(procName, parameters, commandType: CommandType.StoredProcedure)
            };
            int filterlength = parameters.Get<int>("@filterlength");
            result.Filterlength = filterlength;
            int length = parameters.Get<int>("@length");
            result.Length = length;
            return result;
        }


        public int Update(int id, ItemViewModel itemViewModel)
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_UpdateItem";
            parameters.Add("@paramId", id);
            parameters.Add("@paramName", itemViewModel.Name);
            parameters.Add("@paramPrice", itemViewModel.Price);
            parameters.Add("@paramStock", itemViewModel.Stock);
            parameters.Add("@paramSupplier", itemViewModel.Supplier);
            var items = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            return items;
        }
    }
}

