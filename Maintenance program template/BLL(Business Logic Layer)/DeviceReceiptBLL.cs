using Maintenance_program_template.DAL_Data_Access_Layer_;
using Maintenance_program_template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance_program_template.BLL_Business_Logic_Layer_
{
    public class DeviceReceiptBLL
    {
        private DeviceReceiptDAL dal = new DeviceReceiptDAL();

        public List<DeviceReceipt> GetWaitingClients(
     int page,
     int pageSize)
        {
            return dal.GetWaitingClients(
                page,
                pageSize);
        }

        public void MoveToInProgress(int receiptID)
        {
            if (receiptID <= 0)
                throw new Exception("رقم السند غير صالح");

            dal.MoveToInProgress(receiptID);
        }
        public int GetTotalPages(int pageSize)
        {
            int totalRecords =
                dal.GetWaitingClientsCount();

            return (int)Math.Ceiling(
                (double)totalRecords / pageSize);
        }
        public List<DeviceReceipt> SearchWaitingClients(
    string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new Exception("يرجى إدخال كلمة البحث");

            return dal.SearchWaitingClients(keyword);
        }
        public List<DeviceReceipt> GetInProgressDevices(
    int page,
    int pageSize)
        {
            return dal.GetInProgressDevices(
                page,
                pageSize);
        }

        public int GetInProgressTotalPages(
            int pageSize)
        {
            int totalRecords =
                dal.GetInProgressDevicesCount();

            return (int)Math.Ceiling(
                (double)totalRecords / pageSize);
        }

        public void MoveToCompleted(
            int receiptID)
        {
            if (receiptID <= 0)
                throw new Exception(
                    "رقم السند غير صالح");

            dal.MoveToCompleted(receiptID);
        }
        public List<DeviceReceipt> SearchInProgressDevices(
    string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new Exception(
                    "يرجى إدخال كلمة البحث");

            return dal.SearchInProgressDevices(
                keyword);
        }
        public List<DeviceReceipt> GetCompletedDevices(
    int page,
    int pageSize)
        {
            return dal.GetCompletedDevices(
                page,
                pageSize);
        }

        public int GetCompletedPages(
            int pageSize)
        {
            int total =
                dal.GetCompletedCount();

            return (int)Math.Ceiling(
                (double)total / pageSize);
        }

        public List<DeviceReceipt> SearchCompletedDevices(
            string keyword)
        {
            return dal.SearchCompletedDevices(
                keyword);
        }

        public void DeliverDevice(
            int receiptID)
        {
            dal.DeliverDevice(receiptID);
        }
        public List<RepairWarranty> GetRepairs(
    int page,
    int pageSize)
        {
            return dal.GetRepairs(
                page,
                pageSize);
        }
        public int GetRepairsPages(int pageSize)
        {
            int total = dal.GetRepairsCount();

            return (int)Math.Ceiling((double)total / pageSize);
        }

        public List<RepairWarranty> SearchRepairs(string keyword)
        {
            return dal.SearchRepairs(keyword);
        }
    }
}
