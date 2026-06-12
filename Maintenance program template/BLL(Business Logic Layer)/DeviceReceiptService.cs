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
    public class DeviceReceiptService
    {
        private readonly DeviceReceiptRepository _repo = new DeviceReceiptRepository();

        public int AddReceipt(DeviceReceipt receipt)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(receipt.CustomerName))
                throw new Exception("اسم العميل مطلوب");

            if (string.IsNullOrWhiteSpace(receipt.ReceiptNumber))
                throw new Exception("رقم السند مطلوب");

            if (_repo.IsReceiptExists(receipt.ReceiptNumber))
                throw new Exception("رقم السند مسجل مسبقاً");

            return _repo.InsertReceipt(receipt);
        }
        public DataTable GetEmployees()
        {
            return _repo.GetEmployees();
        }
        public DataTable GetReceipts(int pageNumber, int pageSize)
        {
            return _repo.GetReceipts(pageNumber, pageSize);
        }
        public DataTable SearchReceipts(string keyword)
        {
            return _repo.SearchReceipts(keyword);
        }
        public DeviceReceiptSearchResult Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new Exception("أدخل قيمة للبحث");

            return _repo.Search(keyword);
        }
        public void SaveRepair(Repair repair)
        {
            if (repair.ReceiptID == 0)
                throw new Exception("يجب اختيار سند");

            if (repair.EmployeeID == 0)
                throw new Exception("يجب اختيار موظف");

            _repo.SaveRepair(repair);
        }
        public DataTable GetRepairs(int pageNumber, int pageSize)
        {
            return _repo.GetRepairs(pageNumber, pageSize);
        }
        public DataTable SearchRepairs(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                throw new Exception("الرجاء إدخال قيمة البحث");

            return _repo.SearchRepairs(search.Trim());
        }
        public DataTable GetReceiptForWhatsApp(int id)
        {
            return _repo.GetReceiptForWhatsApp(id);
        }
        public DataTable GetReceipts(DateTime? from, DateTime? to, int page, int size)
        {
            return _repo.GetReceiptsPaged(from, to, page, size);
        }

        public int GetReceiptsCount(DateTime? from, DateTime? to)
        {
            return _repo.GetReceiptsCount(from, to);
        }
        public DataTable GetDeliveredDevices(DateTime? from, DateTime? to, int page, int size)
        {
            return _repo.GetRepairsPaged(from, to, page, size);
        }

        public int GetDeliveredDevicesCount(DateTime? from, DateTime? to)
        {
            return _repo.GetRepairsCount(from, to);
        }
        public DataTable GetEmployeeReport(DateTime from, DateTime to, int page, int size)
        {
            if (from > to)
                throw new Exception("تاريخ البداية أكبر من النهاية");

            return _repo.GetEmployeeReport(from, to, page, size);
        }
        public DataTable GetComplaints(DateTime from, DateTime to, int page, int size)
        {
            if (from > to)
                throw new Exception("تاريخ البداية أكبر من النهاية");

            return _repo.GetComplaints(from, to, page, size);
        }
    }
}
