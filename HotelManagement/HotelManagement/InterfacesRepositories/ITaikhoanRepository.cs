using HotelManagement.Models;
namespace HotelManagement.InterfacesRepositories
{
    public interface ITaikhoanRepository
    {
        Task<Taikhoan> GetByUsernameAndPasswordAsync(string username, string password);
        Task<Taikhoan> GetByIdAsync(int id);
    }
}
