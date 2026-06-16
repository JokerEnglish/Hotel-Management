using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repositories
{
    public class LoaiPhongRepository : ILoaiPhongRepository
    {
        private readonly HotelDbContext _dbContext;

        public LoaiPhongRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Loaiphong>> GetAllAsync()
        {
            return await _dbContext.Loaiphongs.ToListAsync();
        }

        public async Task<Loaiphong> GetByIdAsync(int id)
        {
            return await _dbContext.Loaiphongs.FindAsync(id);
        }

        public async Task AddAsync(Loaiphong loaiPhong)
        {
            await _dbContext.Loaiphongs.AddAsync(loaiPhong);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Loaiphong loaiPhong)
        {
            _dbContext.Loaiphongs.Update(loaiPhong);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loaiPhong = await _dbContext.Loaiphongs.FindAsync(id);
            if (loaiPhong != null)
            {
                _dbContext.Loaiphongs.Remove(loaiPhong);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
