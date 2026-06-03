using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repositories
{
    public class PhongRepository : IPhongRepository
    {
        private readonly HotelDbContext _dbContext;
        public PhongRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Phong> GetAllAsync()
        {
            return _dbContext.Phongs.Select(p => new Phong
            {
                Map = p.Map,
                Tenphong = p.Tenphong,
                MaloaiphongNavigation = p.MaloaiphongNavigation,
                Tinhtrang = p.Tinhtrang,
                Ghichu = p.Ghichu,
                Soluongkhachtoida = p.Soluongkhachtoida
            });
        }
        public async Task<Phong> GetByIdAsync(int id)
        {
         return await _dbContext.Phongs.Include(p => p.MaloaiphongNavigation) .FirstOrDefaultAsync(p => p.Map == id);
        }
        public async Task AddAsync(Phong phong)
        {
            await _dbContext.Phongs.AddAsync(phong);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Phong phong)
        {
            _dbContext.Phongs.Update(phong);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var phong = await _dbContext.Phongs.FindAsync(id);
            if (phong != null)
            {
                _dbContext.Phongs.Remove(phong);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<List<Loaiphong>> GetAllLoaiPhong()
        {
            return await _dbContext.Loaiphongs.ToListAsync();
        }
        public async Task<Loaiphong> GetRoomTypeById(int id)
        {
            return await _dbContext.Loaiphongs.FindAsync(id);
        }
        public async Task<List<string>> GetDistinctRoomTypeAsync()
        {
            return await _dbContext.Loaiphongs.Select(l => l.Tenloai).Distinct().ToListAsync();
        }
        public async Task<List<Phong>> GetRoomsByTinhtrangAsync(int tinhtrang, int roomid = 0)
        {
            return await _dbContext.Phongs
                .Where(r => r.Tinhtrang == tinhtrang || r.Map == roomid)
                .ToListAsync();
        }
        public async Task<Phong> GetRoomByNameAsync(string tenphong)
        {
            return await _dbContext.Phongs.FirstOrDefaultAsync(p => p.Tenphong == tenphong);
        }
        public async Task<List<Phong>> GetRoomUsageReportAsync(int thang)
        {
            
            return await _dbContext.Phongs.ToListAsync();
        }
        public async Task<List<int>> GetSoluongkhachtoidaList()
        {
            return await _dbContext.Phongs.Select(p => p.Soluongkhachtoida).Distinct().ToListAsync();
        }
    }
}
