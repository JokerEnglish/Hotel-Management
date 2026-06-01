using HotelManagement.Models;

namespace HotelManagement.InterfacesRepositories
{
    public interface INhanvienRepository
    {
        Task<IEnumerable<Nhanvien>> GetAllAsync();
    }
}
