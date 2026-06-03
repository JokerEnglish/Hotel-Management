using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repositories
{
    public class NhanvienRepository : INhanvienRepository
    {
        private readonly HotelDbContext _context;
        public NhanvienRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Nhanvien>> GetAllAsync()
        {
            return await _context.Nhanviens.Include(n => n.Taikhoans).ToListAsync();
        }

        public async Task AddAsync(Nhanvien nhanvien)
        {
            await _context.Nhanviens.AddAsync(nhanvien);
            await _context.SaveChangesAsync();
        }

        public async Task<Nhanvien?> GetByIdAsync(int id)
        {
            return await _context.Nhanviens.FindAsync(id);
        }

        public async Task UpdateAsync(Nhanvien nhanvien)
        {
            _context.Nhanviens.Update(nhanvien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nv = await _context.Nhanviens
               .Include(n => n.Taikhoans)
               .FirstOrDefaultAsync(n => n.Manv == id);
            if (nv != null)
            {
                // Bước 1: Xóa các tài khoản liên kết của nhân viên này trước
                if (nv.Taikhoans.Any())
                {
                    _context.Taikhoans.RemoveRange(nv.Taikhoans);
                }
                // Bước 2: Tiến hành xóa nhân viên
                _context.Nhanviens.Remove(nv);

                await _context.SaveChangesAsync();
            }
        }
    }
}
