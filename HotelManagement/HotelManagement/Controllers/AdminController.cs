using HotelManagement.InterfacesRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
   
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly INhanvienRepository _nvRepo;

        public AdminController(INhanvienRepository nvRepo)
        {
            _nvRepo = nvRepo;
        }

        public async Task<IActionResult> EmployeeList()
        {
            var nv = await _nvRepo.GetAllAsync();
            return View(nv);
        }
    }
}
