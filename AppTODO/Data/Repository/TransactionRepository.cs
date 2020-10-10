using Dapper;
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
    public class TransactionRepository : ITransactionRepository
    {
        public readonly ConnectionStrings _connectionStrings;
        readonly DynamicParameters parameters = new DynamicParameters();

        public TransactionRepository(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }
        public int Create(TransactionViewModel transactionViewModel)
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_CreateTransaction";
                parameters.Add("@paramQuantity", transactionViewModel.Quantity);
                parameters.Add("@paramTotalPrice", transactionViewModel.TotalPrice);
                parameters.Add("@paramTransactionDate", transactionViewModel.TransactionDate);
                parameters.Add("@paramItem", transactionViewModel.Item);
                var transactions = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return transactions;
        }

        public int Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_DeleteTransaction";
                parameters.Add("@paramId", id);
                var transactions = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return transactions;         
        }

        public async Task<IEnumerable<TransactionViewModel>> Get()
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_GetAllTransaction";
            var transactions = await connection.QueryAsync<TransactionViewModel>(procName, commandType: CommandType.StoredProcedure);
            return transactions;

        }

        public async Task<ResponseTransaction> Paging(string keyword, int pageSize, int pageNumber)
        {
            using SqlConnection connection = new SqlConnection(_connectionStrings.Value);
            var procName = "SP_FilterDataTransaction";
            parameters.Add("@paramKeyword", keyword);
            parameters.Add("@paramPageSize", pageSize);
            parameters.Add("@paramPageNumber", pageNumber);
            parameters.Add("@length", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@filterlength", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = new ResponseTransaction
            {
                Data = await connection.QueryAsync<TransactionViewModel>(procName, parameters, commandType: CommandType.StoredProcedure)
            };

            int filterlength = parameters.Get<int>("@filterlength");
            result.Filterlength = filterlength;
            int length = parameters.Get<int>("@length");
            result.Length = length;
            return result;
        }
    }
}
