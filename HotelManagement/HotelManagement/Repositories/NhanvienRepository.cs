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
            return await _context.Nhanviens.ToListAsync();
        }
    }
}
