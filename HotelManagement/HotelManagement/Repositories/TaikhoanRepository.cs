using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repositories
{
    public class TaikhoanRepository : ITaikhoanRepository
    {
        private readonly HotelDbContext _dbContext;

        public TaikhoanRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Taikhoan> GetByUsernameAndPasswordAsync(string username, string password)
        {
            return await _dbContext.Taikhoans
                .Include(t => t.ManvNavigation) // KÉO DATA TỪ BẢNG NHÂN VIÊN SANG
                .FirstOrDefaultAsync(t => t.Tentknv.Trim() == username.Trim() && t.Mktk.Trim() == password.Trim());
        }

        public async Task<Taikhoan> GetByIdAsync(int id)
        {
            return await _dbContext.Taikhoans.FindAsync(id);
        }

        public async Task AddAsync(Taikhoan taikhoan)
        {
            await _dbContext.Taikhoans.AddAsync(taikhoan);
            await _dbContext.SaveChangesAsync();
        }

    }
}
