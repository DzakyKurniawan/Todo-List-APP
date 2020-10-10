using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interface
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> Get();
        Task<IEnumerable<Supplier>> Get(int id);
        int Create(SupplierViewModel supplierViewModel);
        int Update(int id, SupplierViewModel supplierViewModel);
        int Delete(int id);
    }
}
