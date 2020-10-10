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
    public class SupplierService : ISupplierService
    {
        private ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public int Create(SupplierViewModel supplierViewModel)
        {
            return _supplierRepository.Create(supplierViewModel);
        }

        public int Delete(int id)
        {
            return _supplierRepository.Delete(id);
        }

        public Task<IEnumerable<Supplier>> Get(int id)
        {
            return _supplierRepository.Get(id);
        }

        public IEnumerable<Supplier> Get()
        {
            return _supplierRepository.Get();
        }

        public int Update(int id, SupplierViewModel supplierViewModel)
        {
            return _supplierRepository.Update(id, supplierViewModel);
        }
    }
}
