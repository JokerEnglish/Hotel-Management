using HotelManagement.InterfacesRepositories;
using HotelManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    [Authorize] // Bắt buộc phải đăng nhập
    public class RoomController : Controller
    {
        private readonly IPhongRepository _phongRepo;

        public RoomController(IPhongRepository phongRepo)
        {
            _phongRepo = phongRepo;
        }

        // --- TẤT CẢ NHÂN VIÊN ĐỀU ĐƯỢC XEM ---
        public async Task<IActionResult> RoomList(string searchString, int pageNumber = 1)
        {
            int pageSize = 10;
            var phongs = _phongRepo.GetAllAsync();

            // Nếu người dùng có nhập từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                phongs = phongs.Where(p => p.Tenphong.Contains(searchString));
            }

            var paginatedList = await PaginatedList<Models.Phong>.CreateAsync(phongs, pageNumber, pageSize);

            // Lưu lại từ khóa tìm kiếm để hiển thị lại trên giao diện
            ViewData["CurrentFilter"] = searchString;

            return View(paginatedList);
        }


        // --- CHỈ ADMIN MỚI ĐƯỢC THÊM PHÒNG ---
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.LoaiPhongList = await _phongRepo.GetAllLoaiPhong();
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(Models.Phong phong)
        {
            ModelState.Remove("MaloaiphongNavigation");
            if (ModelState.IsValid)
            {
                await _phongRepo.AddAsync(phong);
                return RedirectToAction("RoomList");
            }
            ViewBag.LoaiPhongList = await _phongRepo.GetAllLoaiPhong();
            return View(phong);
        }

        // --- CHỈ ADMIN MỚI ĐƯỢC SỬA PHÒNG ---
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var phong = await _phongRepo.GetByIdAsync(id);
            if (phong == null) return NotFound();

            ViewBag.LoaiPhongList = await _phongRepo.GetAllLoaiPhong();
            return View(phong);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Models.Phong phong)
        {
            if (id != phong.Map) return NotFound();

            ModelState.Remove("MaloaiphongNavigation");
            if (ModelState.IsValid)
            {
                await _phongRepo.UpdateAsync(phong);
                return RedirectToAction("RoomList");
            }
            ViewBag.LoaiPhongList = await _phongRepo.GetAllLoaiPhong();
            return View(phong);
        }

        // --- CHỈ ADMIN MỚI ĐƯỢC XÓA PHÒNG ---
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _phongRepo.DeleteAsync(id);
            return RedirectToAction("RoomList");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var phong = await _phongRepo.GetByIdAsync(id);
            if (phong == null)
            {
                return NotFound();
            }
            return View(phong);
        }
    }
}
