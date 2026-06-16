using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    // Bắt buộc đăng nhập và chỉ có Role là ADMIN mới được truy cập
    [Authorize(Roles = "ADMIN")]
    public class LoaiPhongController : Controller
    {
        private readonly ILoaiPhongRepository _loaiPhongRepo;

        public LoaiPhongController(ILoaiPhongRepository loaiPhongRepo)
        {
            _loaiPhongRepo = loaiPhongRepo;
        }

        // 1. Xem danh sách Loại Phòng (có hỗ trợ tìm kiếm)
        public async Task<IActionResult> Index(string searchString)
        {
            var loaiPhongs = await _loaiPhongRepo.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                // Lọc danh sách loại phòng theo tên (bỏ qua viết hoa/thường)
                loaiPhongs = loaiPhongs.Where(l => l.Tenloai.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ViewData["CurrentFilter"] = searchString;
            return View(loaiPhongs);
        }
        // 2. Thêm Loại Phòng (Hiện form)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. Thêm Loại Phòng (Xử lý lưu vào DB)
        [HttpPost]
        public async Task<IActionResult> Create(Loaiphong loaiPhong)
        {
            // Bỏ qua validate danh sách Phòng liên quan
            ModelState.Remove("Phongs");

            if (ModelState.IsValid)
            {
                await _loaiPhongRepo.AddAsync(loaiPhong);
                return RedirectToAction(nameof(Index));
            }
            return View(loaiPhong);
        }

        // 4. Sửa Loại Phòng (Hiện form kèm dữ liệu cũ)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var loaiPhong = await _loaiPhongRepo.GetByIdAsync(id);
            if (loaiPhong == null) return NotFound();
            return View(loaiPhong);
        }

        // 5. Sửa Loại Phòng (Xử lý lưu thay đổi)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Loaiphong loaiPhong)
        {
            if (id != loaiPhong.Maloaiphong) return NotFound();

            ModelState.Remove("Phongs");

            if (ModelState.IsValid)
            {
                await _loaiPhongRepo.UpdateAsync(loaiPhong);
                return RedirectToAction(nameof(Index));
            }
            return View(loaiPhong);
        }

        // 6. Xóa Loại Phòng
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _loaiPhongRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
