using Maintenance_program_template.Models;
using System;
using System.Data;

public class SparePartService
{
    private readonly SparePartRepository _repo = new SparePartRepository();

    public void AddSparePart(string name, int quantity, string supplier, decimal price)
    {
        // Business Validation
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("اسم القطعة مطلوب");

        if (quantity <= 0)
            throw new Exception("الكمية يجب أن تكون أكبر من صفر");

        if (string.IsNullOrWhiteSpace(supplier))
            throw new Exception("المورد مطلوب");

        if (price <= 0)
            throw new Exception("السعر يجب أن يكون أكبر من صفر");

        // Call DAL
        _repo.AddSparePart(name, quantity, supplier, price);
    }
    public int GetNextPartId()
    {
        return _repo.GetNextPartId();
    }
    public DataTable GetSuppliers()
    {
        return _repo.GetSuppliers();
    }
    public DataTable GetParts(int pageNumber, int pageSize)
    {
        return _repo.GetParts(pageNumber, pageSize);
    }

    //public DataTable GetAllParts()
    //{
    //    return _repo.GetAllParts(); // إذا احتجته لاحقاً
    //}
    public SparePart SearchPiece(string name)
    {
        return _repo.SearchByName(name);
    }
    public void UpdatePart(SparePart part)
    {
        _repo.UpdatePart(part);
    }

    //public bool IsPartUsed(int id)
    //{
    //    return _repo.IsPartUsed(id);
    //}

    public void DeletePart(int id)
    {
        _repo.DeletePart(id);
    }
    public DataTable GetSpareParts()
    {
        return _repo.GetSpareParts();
    }
    public decimal GetPartPrice(int partId)
    {
        return _repo.GetPartPrice(partId);
    }
}