using HotelManagement.Models;

namespace HotelManagement.InterfacesRepositories
{
    public interface ILoaiKhachRepository
    {
        Task<List<Loaikhach>> GetAllAsync();
        Task<Loaikhach> GetByIdAsync(int id);
        Task AddAsync(Loaikhach loaiKhach);
        Task UpdateAsync(Loaikhach loaiKhach);
        Task DeleteAsync(int id);
    }
}
