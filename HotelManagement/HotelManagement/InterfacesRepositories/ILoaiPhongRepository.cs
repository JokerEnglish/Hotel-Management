using HotelManagement.Models;

namespace HotelManagement.InterfacesRepositories
{
    public interface ILoaiPhongRepository
    {
        Task<IEnumerable<Loaiphong>> GetAllAsync();
        Task<Loaiphong> GetByIdAsync(int id);
        Task AddAsync(Loaiphong loaiPhong);
        Task UpdateAsync(Loaiphong loaiPhong);
        Task DeleteAsync(int id);
    }
}
