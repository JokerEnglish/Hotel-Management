using HotelManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HotelManagement.InterfacesRepositories;
using HotelManagement.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPhongRepository _phongRepo;

        public HomeController(IPhongRepository phongRepo)
        {
            _phongRepo = phongRepo;
        }

        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 10;
            var phongs = _phongRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                phongs = phongs.Where(p => p.Tenphong.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;

            var paginatedList = await PaginatedList<Phong>.CreateAsync(phongs, pageNumber, pageSize);

            return View(paginatedList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
