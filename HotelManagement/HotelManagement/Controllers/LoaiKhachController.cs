using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class LoaiKhachController : Controller
    {
        private readonly ILoaiKhachRepository _loaiKhachRepo;

        public LoaiKhachController(ILoaiKhachRepository loaiKhachRepo)
        {
            _loaiKhachRepo = loaiKhachRepo;
        }

        // Danh sách Loại Khách (có tìm kiếm)
        public async Task<IActionResult> Index(string searchString)
        {
            var loaiKhachs = await _loaiKhachRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                loaiKhachs = loaiKhachs
                    .Where(l => l.Tenloaikhach != null &&
                           l.Tenloaikhach.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewData["CurrentFilter"] = searchString;
            return View(loaiKhachs);
        }

        // Thêm mới (GET)
        [HttpGet]
        public IActionResult Create() => View();

        // Thêm mới (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Loaikhach loaiKhach)
        {
            ModelState.Remove("Khachhangs");
            if (ModelState.IsValid)
            {
                await _loaiKhachRepo.AddAsync(loaiKhach);
                return RedirectToAction(nameof(Index));
            }
            return View(loaiKhach);
        }

        // Sửa (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var loaiKhach = await _loaiKhachRepo.GetByIdAsync(id);
            if (loaiKhach == null) return NotFound();
            return View(loaiKhach);
        }

        // Sửa (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Loaikhach loaiKhach)
        {
            if (id != loaiKhach.Maloaikhach) return NotFound();
            ModelState.Remove("Khachhangs");
            if (ModelState.IsValid)
            {
                await _loaiKhachRepo.UpdateAsync(loaiKhach);
                return RedirectToAction(nameof(Index));
            }
            return View(loaiKhach);
        }

        // Xóa
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _loaiKhachRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
