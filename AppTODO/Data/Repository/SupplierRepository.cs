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
    public class SupplierRepository : ISupplierRepository
    {
        public readonly ConnectionStrings _connectionStrings;
        DynamicParameters parameters = new DynamicParameters();
        public SupplierRepository(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public int Create(SupplierViewModel supplierViewModel)
        {
            using(SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_CreateSupplier";
                parameters.Add("@paramName", supplierViewModel.Name);
                parameters.Add("@paramJoinDate", supplierViewModel.JoinDate);
                var suppliers = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return suppliers;
            }
        }

        public int Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_DeleteSupplier";
                parameters.Add("@paramId", id);
                var suppliers = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return suppliers;
            }
        }

        public IEnumerable<Supplier> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetAllSupplier";
                var suppliers = connection.Query<Supplier>(procName, commandType: CommandType.StoredProcedure);
                return suppliers;
            }
        }

            public Task<IEnumerable<Supplier>> Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetIdSupplier";
                parameters.Add("@paramId", id);
                var suppliers = connection.QueryAsync<Supplier>(procName, parameters, commandType: CommandType.StoredProcedure);
                return suppliers;
            }
        }

        public int Update(int id, SupplierViewModel supplierViewModel)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_UpdateSupplier";
                parameters.Add("@paramId", id);
                parameters.Add("@paramName", supplierViewModel.Name);
                parameters.Add("@paramJoinDate", supplierViewModel.JoinDate);
                var suppliers = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return suppliers;
            }
        }
    }
}
