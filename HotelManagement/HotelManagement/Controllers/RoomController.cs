using HotelManagement.InterfacesRepositories;
using HotelManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IPhongRepository _phongRepo;

        public RoomController(IPhongRepository phongRepo)
        {
            _phongRepo = phongRepo;
        }

        public async Task<IActionResult> RoomList(int pageNumber = 1)
        {
            int pageSize = 10;
            var phongs = _phongRepo.GetAllAsync();
            var paginatedList = await PaginatedList<Models.Phong>.CreateAsync(phongs, pageNumber, pageSize);
            return View(paginatedList);
        }
    }
}
