using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class PhuthuController : Controller
    {
        private readonly IPhuthuRepository _phuthuRepo;

        public PhuthuController(IPhuthuRepository phuthuRepo)
        {
            _phuthuRepo = phuthuRepo;
        }

        // Danh sách phụ thu
        public async Task<IActionResult> Index()
        {
            var phuthus = await _phuthuRepo.GetAllPhuthusAsync();
            return View(phuthus);
        }

        // Thêm mới (GET)
        [HttpGet]
        public IActionResult Create() => View();

        // Thêm mới (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phuthu phuthu)
        {
            if (ModelState.IsValid)
            {
                await _phuthuRepo.AddPhuthuAsync(phuthu);
                return RedirectToAction(nameof(Index));
            }
            return View(phuthu);
        }

        // Sửa (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var phuthu = await _phuthuRepo.GetPhuthuByIdAsync(id);
            if (phuthu == null) return NotFound();
            return View(phuthu);
        }

        // Sửa (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Phuthu phuthu)
        {
            if (id != phuthu.Idphuthu) return NotFound();
            if (ModelState.IsValid)
            {
                await _phuthuRepo.UpdatePhuthuAsync(phuthu);
                return RedirectToAction(nameof(Index));
            }
            return View(phuthu);
        }

        // Xóa
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _phuthuRepo.DeletePhuthuAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
