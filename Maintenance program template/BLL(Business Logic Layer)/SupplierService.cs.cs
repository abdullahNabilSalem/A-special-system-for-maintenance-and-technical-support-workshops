using Maintenance_program_template.DAL_Data_Access_Layer_;
using Maintenance_program_template.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    public class SupplierService
    {
        private readonly SupplierRepository _repo = new SupplierRepository();

        public DataTable GetSuppliers(int pageNumber, int pageSize)
        {
            return _repo.GetSuppliers(pageNumber, pageSize);
        }
        public void AddSupplier(Supplier supplier)
        {
            // Validation (Business Rules)
            if (string.IsNullOrWhiteSpace(supplier.SupplierName))
                throw new Exception("اسم الشركة مطلوب");

            if (string.IsNullOrWhiteSpace(supplier.SupplierPhone))
                throw new Exception("رقم الهاتف مطلوب");

            _repo.AddSupplier(supplier);
        }
        public Supplier SearchSupplier(string name)
        {
            return _repo.GetSupplierByName(name);
        }
        public void UpdateSupplier(Supplier supplier)
        {
            if (supplier.SupplierID <= 0)
                throw new Exception("يجب البحث عن المورد أولاً");

            if (string.IsNullOrWhiteSpace(supplier.SupplierName))
                throw new Exception("اسم الشركة مطلوب");

            _repo.UpdateSupplier(supplier);
        }
        public void DeleteSupplier(int id)
        {
            _repo.DeleteSupplier(id);
        }
    }
}
