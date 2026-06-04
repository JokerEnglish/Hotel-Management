using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagement.Repositories
{
    public class PhuthuRepository : IPhuthuRepository
    {
        private readonly HotelDbContext _dbContext;

        public PhuthuRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Phuthu>> GetAllPhuthusAsync()
        {
            return await _dbContext.Phuthus.ToListAsync();
        }

        public async Task<Phuthu> GetPhuthuByIdAsync(double id)
        {
            return await _dbContext.Phuthus.FindAsync((int)id);
        }

        public async Task AddPhuthuAsync(Phuthu phuthu)
        {
            await _dbContext.Phuthus.AddAsync(phuthu);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePhuthuAsync(Phuthu phuthu)
        {
            _dbContext.Phuthus.Update(phuthu);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePhuthuAsync(double id)
        {
            var phuthu = await _dbContext.Phuthus.FindAsync((int)id);
            if (phuthu != null)
            {
                _dbContext.Phuthus.Remove(phuthu);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> PhuthuExistsAsync(double id)
        {
            return await _dbContext.Phuthus.AnyAsync(p => p.Idphuthu == (int)id);
        }

        public async Task<Phuthu> GetFirstPhuthuAsync()
        {
            return await _dbContext.Phuthus.FirstOrDefaultAsync();
        }
    }
}
