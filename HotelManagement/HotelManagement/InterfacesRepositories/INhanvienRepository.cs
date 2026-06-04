using HotelManagement.Models;

namespace HotelManagement.InterfacesRepositories
{
    public interface INhanvienRepository
    {
        Task<IEnumerable<Nhanvien>> GetAllAsync();
        Task AddAsync(Nhanvien nhanvien);

        Task<Nhanvien?> GetByIdAsync(int id);
        Task UpdateAsync(Nhanvien nhanvien);
        Task DeleteAsync(int id);
        Task<Nhanvien> GetEmployeeByIdAsync(int id);
        
    }
}
