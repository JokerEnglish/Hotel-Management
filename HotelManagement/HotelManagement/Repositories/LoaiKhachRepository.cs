using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repositories
{
    public class LoaiKhachRepository : ILoaiKhachRepository
    {
        private readonly HotelDbContext _dbContext;

        public LoaiKhachRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Loaikhach>> GetAllAsync()
        {
            return await _dbContext.Loaikhaches.ToListAsync();
        }

        public async Task<Loaikhach> GetByIdAsync(int id)
        {
            return await _dbContext.Loaikhaches.FindAsync(id);
        }

        public async Task AddAsync(Loaikhach loaiKhach)
        {
            await _dbContext.Loaikhaches.AddAsync(loaiKhach);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Loaikhach loaiKhach)
        {
            _dbContext.Loaikhaches.Update(loaiKhach);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loaiKhach = await _dbContext.Loaikhaches.FindAsync(id);
            if (loaiKhach != null)
            {
                _dbContext.Loaikhaches.Remove(loaiKhach);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
