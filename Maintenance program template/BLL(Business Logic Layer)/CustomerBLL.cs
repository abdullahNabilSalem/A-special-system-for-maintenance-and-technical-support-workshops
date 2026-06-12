using Maintenance_program_template.DAL_Data_Access_Layer_;
using Maintenance_program_template.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    public class CustomerBLL
    {
        private CustomerDAL dal = new CustomerDAL();

        public List<Customer> GetCustomers()
        {
            return dal.GetAllCustomers();
        }
        public List<Customer> GetCustomersAll(int page, int pageSize)
        {
            return dal.GetCustomers(page, pageSize);
        }

        public int GetTotalPages(int pageSize)
        {
            int totalRecords = dal.GetCustomersCount();

            return (int)Math.Ceiling((double)totalRecords / pageSize);
        }
        public void AddCustomer(Customer customer)
        {
            // ✅ تحقق بسيط
            if (string.IsNullOrWhiteSpace(customer.CustomerName))
                throw new Exception("اسم العميل مطلوب");

            dal.AddCustomer(customer);
        }
        public DataTable SearchCustomers(string customerName)
        {
            return dal.SearchCustomers(customerName);
        }
        public Customer SearchByName(string name)
        {
            return dal.GetCustomerByName(name);
        }

        public void Update(Customer c)
        {
            dal.Update(c);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }
        public List<Customer> GetCustomersPaged(
            int page,
            int pageSize,
            string search)
        {
            return dal.GetCustomersPaged(
                page,
                pageSize,
                search);
        }

        public int GetCustomersCount(string search)
        {
            return dal.GetCustomersCount(search);
        }
    }
}
