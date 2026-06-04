using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagement.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly HotelDbContext _dbContext;

        public BillRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Hoadon>> GetAllBills()
        {
            return await _dbContext.Hoadons.Include(h => h.ManvNavigation).ToListAsync();
        }

        public async Task<Hoadon> GetBillById(int id)
        {
            return await _dbContext.Hoadons.Include(h => h.ManvNavigation).FirstOrDefaultAsync(h => h.Mahd == id);
        }

        public async Task CreateBill(Hoadon hoadon)
        {
            await _dbContext.Hoadons.AddAsync(hoadon);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBill(Hoadon hoadon)
        {
            _dbContext.Hoadons.Update(hoadon);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBill(int id)
        {
            var hoadon = await _dbContext.Hoadons.FindAsync(id);
            if (hoadon != null)
            {
                _dbContext.Hoadons.Remove(hoadon);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> BillExists(int id)
        {
            return await _dbContext.Hoadons.AnyAsync(h => h.Mahd == id);
        }
    }
}
