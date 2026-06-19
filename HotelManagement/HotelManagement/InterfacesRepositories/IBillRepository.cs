using HotelManagement.Models;
using HotelManagement.ViewModels;

public interface IBillRepository
{
    Task<List<Hoadon>> GetAllBills();
    Task<Hoadon> GetBillById(int id);
    Task CreateBill(Hoadon hoadon);
    Task UpdateBill(Hoadon hoadon);
    Task DeleteBill(int id);
    Task<bool> BillExists(int id);

    // ===== Báo Cáo Doanh Thu =====
    Task<DoanhThuFilterViewModel> GetDoanhThuAsync(DateTime? tuNgay, DateTime? denNgay);
    Task<byte[]> ExportDoanhThuExcelAsync(DateTime? tuNgay, DateTime? denNgay);
}