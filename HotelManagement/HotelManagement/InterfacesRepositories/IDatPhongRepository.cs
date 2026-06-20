using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagement.Models;

namespace HotelManagement.InterfacesRepositories
{
    public interface IDatPhongRepository
    {
        Task<IEnumerable<DatPhong>> GetAllAsync();
        Task<IEnumerable<DatPhong>> GetByCustomerIdAsync(int customerId);
        Task<DatPhong> GetByIdAsync(int id);
        Task AddAsync(DatPhong datPhong);
        Task UpdateAsync(DatPhong datPhong);
        Task DeleteAsync(int id);
    }
}
