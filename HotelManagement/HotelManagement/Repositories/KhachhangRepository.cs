using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Repositories
{
    public class KhachhangRepository : IKhachhangRepository
    {
        private readonly HotelDbContext _dbContext;

        public KhachhangRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Khachhang> GetByIdAsync(int id)
        {
            return await _dbContext.Khachhangs
                .Include(k => k.MaloaikhachNavigation)
                .Include(k => k.MapNavigation)
                .FirstOrDefaultAsync(k => k.Makh == id);
        }

        public IQueryable<Khachhang> GetAllAsync()
        {
            return _dbContext.Khachhangs
                .Include(k => k.MaloaikhachNavigation)
                .Include(k => k.MapNavigation);
        }

        public async Task AddAsync(Khachhang khach)
        {
            await _dbContext.Khachhangs.AddAsync(khach);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Khachhang khach, int id)
        {
            var existing = await _dbContext.Khachhangs.FindAsync(id);
            if (existing != null)
            {
                existing.Tenkh = khach.Tenkh;
                existing.Tuoi = khach.Tuoi;
                existing.Tel = khach.Tel;
                existing.Diachikh = khach.Diachikh;
                existing.Cmndkh = khach.Cmndkh;
                existing.Maloaikhach = khach.Maloaikhach;
                existing.Map = khach.Map;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int Id)
        {
            var khach = await _dbContext.Khachhangs.FindAsync(Id);
            if (khach != null)
            {
                _dbContext.Khachhangs.Remove(khach);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Loaikhach>> GetAllLoaikhach()
        {
            return await _dbContext.Loaikhaches.ToListAsync();
        }

        public async Task<Loaikhach> GetClientTypeById(int id)
        {
            return await _dbContext.Loaikhaches.FindAsync(id);
        }

        public async Task<List<string>> GetDistinctClientTypeAsync()
        {
            return await _dbContext.Loaikhaches.Select(lk => lk.Tenloaikhach).ToListAsync();
        }

        public async Task<List<Khachhang>> GetCustomersByRoomIdAsync(int roomId)
        {
            return await _dbContext.Khachhangs.Where(k => k.Map == roomId).ToListAsync();
        }

        public async Task<Khachhang> GetClientByCCCDAsync(string cccd)
        {
            if (string.IsNullOrEmpty(cccd)) return null;
            return await _dbContext.Khachhangs.FirstOrDefaultAsync(k => k.Cmndkh.Trim() == cccd.Trim());
        }

        public async Task<Khachhang> GetClientByIDAsync(int id)
        {
            return await _dbContext.Khachhangs.FindAsync(id);
        }

        public async Task<int> GetSoLuongKhachByMapAsync(int map)
        {
            return await _dbContext.Khachhangs.CountAsync(k => k.Map == map);
        }

        public async Task<IEnumerable<Khachhang>> GetCustomersByRoomNameAsync(string tenPhong)
        {
            return await _dbContext.Khachhangs.Where(k => k.MapNavigation.Tenphong.Trim() == tenPhong.Trim()).ToListAsync();
        }

        public async Task DeleteCustomersByRoomAsync(string tenPhong, string CCCD)
        {
            var customers = await _dbContext.Khachhangs
                .Where(k => k.MapNavigation.Tenphong.Trim() == tenPhong.Trim() && k.Cmndkh.Trim() == CCCD.Trim())
                .ToListAsync();
            
            // Thay vì xóa tài khoản, chỉ cần gỡ bỏ phòng (Map = null) để khách hàng có thể tiếp tục đăng nhập
            foreach (var c in customers)
            {
                c.Map = null;
            }
            _dbContext.Khachhangs.UpdateRange(customers);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetClientIdByName(string tenKhachHang)
        {
            var customer = await _dbContext.Khachhangs.FirstOrDefaultAsync(k => k.Tenkh == tenKhachHang);
            return customer != null ? customer.Makh : 0;
        }
    }
}
