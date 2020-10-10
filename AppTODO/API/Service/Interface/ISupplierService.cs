using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service.Interface
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> Get();
        Task<IEnumerable<Supplier>> Get(int id);
        int Create(SupplierViewModel supplierViewModel);
        int Update(int id, SupplierViewModel supplierViewModel);
        int Delete(int id);
    }
}
