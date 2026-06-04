using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Repositories
{
    public class RentRepository : IRentRepository
    {
        private readonly HotelDbContext _dbContext;

        public RentRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Phieuthue>> GetAllAsync()
        {
            return await _dbContext.Phieuthues
                .Include(p => p.MakhNavigation)
                .Include(p => p.MapNavigation)
                .ToListAsync();
        }

        public async Task<Phieuthue> GetByIdAsync(int id)
        {
            return await _dbContext.Phieuthues
                .Include(p => p.MakhNavigation)
                .Include(p => p.MapNavigation)
                .FirstOrDefaultAsync(p => p.Mapt == id);
        }

        public async Task AddAsync(Phieuthue rent)
        {
            await _dbContext.Phieuthues.AddAsync(rent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Phieuthue rent)
        {
            _dbContext.Phieuthues.Update(rent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rent = await _dbContext.Phieuthues.FindAsync(id);
            if (rent != null)
            {
                _dbContext.Phieuthues.Remove(rent);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> RentExists(int id)
        {
            return await _dbContext.Phieuthues.AnyAsync(e => e.Mapt == id);
        }

        public async Task<List<int>> GetDistinctMakhAsync()
        {
            return await _dbContext.Phieuthues.Select(r => r.Makh).Distinct().ToListAsync();
        }

        public async Task<List<int>> GetDistinctMapAsync()
        {
            return await _dbContext.Phieuthues.Select(r => r.Map).Distinct().ToListAsync();
        }

        public async Task<List<Phieuthue>> GetRentsByCustomerIdAsync(int makh)
        {
            return await _dbContext.Phieuthues.Where(r => r.Makh == makh).ToListAsync();
        }

        public async Task<Phieuthue> GetClientByCCCDAsync(string cccd)
        {
            return await _dbContext.Phieuthues
                .Include(r => r.MakhNavigation)
                .FirstOrDefaultAsync(r => r.Cccd == cccd);
        }

        public async Task<List<Phieuthue>> GetRentsCustomerIdAsync(int customerId)
        {
            return await _dbContext.Phieuthues
                .Include(r => r.MapNavigation)
                .Include(r => r.MapNavigation.MaloaiphongNavigation)
                .Where(r => r.Makh == customerId)
                .ToListAsync();
        }

        public async Task<Phieuthue> GetRentByIDAsync(int makh)
        {
            return await _dbContext.Phieuthues
                .Include(r => r.MapNavigation)
                .Include(r => r.MapNavigation.MaloaiphongNavigation)
                .FirstOrDefaultAsync(r => r.Makh == makh);
        }

        public async Task<Phieuthue> GetRentAsync(int mapt)
        {
            return await _dbContext.Phieuthues
                .Include(r => r.MapNavigation)
                .Include(r => r.MapNavigation.MaloaiphongNavigation)
                .FirstOrDefaultAsync(r => r.Mapt == mapt);
        }

        public async Task<List<Phieuthue>> GetRentByCCCDAsync(string cccd)
        {
            return await _dbContext.Phieuthues
                .Include(r => r.MapNavigation)
                .Include(r => r.MapNavigation.MaloaiphongNavigation)
                .Where(r => r.Cccd == cccd)
                .ToListAsync();
        }

        public async Task<List<Phieuthue>> GetAllRentsByCCCDAsync(string cccd)
        {
            return await _dbContext.Phieuthues.Where(r => r.Cccd == cccd).ToListAsync();
        }

        public async Task<bool> CheckCCCDExistsInPhieuthueAsync(string cccd)
        {
            return await _dbContext.Phieuthues.AnyAsync(r => r.Cccd == cccd);
        }
    }
}
