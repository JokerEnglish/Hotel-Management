using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repositories
{
    public class DatPhongRepository : IDatPhongRepository
    {
        private readonly HotelDbContext _dbContext;

        public DatPhongRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DatPhong>> GetAllAsync()
        {
            return await _dbContext.DatPhongs
                .Include(dp => dp.MakhNavigation)
                .Include(dp => dp.MapNavigation)
                .OrderByDescending(dp => dp.NgayDat)
                .ToListAsync();
        }

        public async Task<IEnumerable<DatPhong>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbContext.DatPhongs
                .Include(dp => dp.MakhNavigation)
                .Include(dp => dp.MapNavigation)
                .Where(dp => dp.Makh == customerId)
                .OrderByDescending(dp => dp.NgayDat)
                .ToListAsync();
        }

        public async Task<DatPhong> GetByIdAsync(int id)
        {
            return await _dbContext.DatPhongs
                .Include(dp => dp.MakhNavigation)
                .Include(dp => dp.MapNavigation)
                .FirstOrDefaultAsync(dp => dp.MaDp == id);
        }

        public async Task AddAsync(DatPhong datPhong)
        {
            await _dbContext.DatPhongs.AddAsync(datPhong);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(DatPhong datPhong)
        {
            _dbContext.DatPhongs.Update(datPhong);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var datPhong = await _dbContext.DatPhongs.FindAsync(id);
            if (datPhong != null)
            {
                _dbContext.DatPhongs.Remove(datPhong);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
