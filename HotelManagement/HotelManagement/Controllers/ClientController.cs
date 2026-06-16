using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using HotelManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagement.Controllers
{
    // Bắt buộc người dùng phải đăng nhập mới được vào quản lý khách
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IKhachhangRepository _khachhangRepo;

        public ClientController(IKhachhangRepository khachhangRepo)
        {
            _khachhangRepo = khachhangRepo;
        }

        // 1. Xem danh sách Khách hàng (Hỗ trợ tìm kiếm, lọc loại khách và phân trang)
        public async Task<IActionResult> Index(string searchString, int? loaiKhachId, int pageNumber = 1)
        {
            int pageSize = 10; // Số khách hàng mỗi trang
            var khachhangs = _khachhangRepo.GetAllAsync();

            // Xử lý Tìm kiếm theo Tên khách hàng
            if (!string.IsNullOrEmpty(searchString))
            {
                khachhangs = khachhangs.Where(k => k.Tenkh.Contains(searchString));
            }

            // Xử lý Lọc theo Loại Khách
            if (loaiKhachId.HasValue)
            {
                khachhangs = khachhangs.Where(k => k.Maloaikhach == loaiKhachId.Value);
            }

            // Chuẩn bị dữ liệu cho Dropdown Chọn Loại Khách
            var loaiKhachList = await _khachhangRepo.GetAllLoaikhach();
            ViewData["LoaiKhachId"] = new SelectList(loaiKhachList, "Maloaikhach", "Tenloaikhach", loaiKhachId);
            ViewData["CurrentFilter"] = searchString; // Lưu lại từ khóa tìm kiếm

            // Tạo danh sách phân trang
            var paginatedList = await PaginatedList<Khachhang>.CreateAsync(khachhangs, pageNumber, pageSize);

            return View(paginatedList);
        }

        // 2. Thêm Khách Hàng mới (Hiển thị Form)
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Lấy danh sách Loại khách để đổ vào Dropdown
            var loaiKhachList = await _khachhangRepo.GetAllLoaikhach();
            ViewData["LoaiKhachId"] = new SelectList(loaiKhachList, "Maloaikhach", "Tenloaikhach");

            // Tạm thời set Map = 1 (Phòng mặc định) nếu bạn không muốn bắt buộc chọn phòng lúc thêm khách
            return View();
        }

        // 3. Xử lý Thêm Khách Hàng vào DB
        [HttpPost]
        public async Task<IActionResult> Create(Khachhang khach)
        {
            // Bỏ qua validate các khóa ngoại không cần nhập từ form
            ModelState.Remove("MaloaikhachNavigation");
            ModelState.Remove("MapNavigation");
            ModelState.Remove("Phieuthues");

            if (ModelState.IsValid)
            {
                // Nếu bạn không có ô nhập Phòng trên form Create, có thể gán mặc định Map = 1 hoặc id phòng nào đó
                if (khach.Map == 0) khach.Map = 1;

                await _khachhangRepo.AddAsync(khach);
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi, load lại dropdown
            var loaiKhachList = await _khachhangRepo.GetAllLoaikhach();
            ViewData["LoaiKhachId"] = new SelectList(loaiKhachList, "Maloaikhach", "Tenloaikhach", khach.Maloaikhach);
            return View(khach);
        }

        // 4. Sửa Thông tin Khách (Hiển thị Form)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var khach = await _khachhangRepo.GetByIdAsync(id);
            if (khach == null) return NotFound();

            var loaiKhachList = await _khachhangRepo.GetAllLoaikhach();
            ViewData["LoaiKhachId"] = new SelectList(loaiKhachList, "Maloaikhach", "Tenloaikhach", khach.Maloaikhach);
            return View(khach);
        }

        // 5. Xử lý Cập nhật Thông tin
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Khachhang khach)
        {
            if (id != khach.Makh) return NotFound();

            ModelState.Remove("MaloaikhachNavigation");
            ModelState.Remove("MapNavigation");
            ModelState.Remove("Phieuthues");

            if (ModelState.IsValid)
            {
                await _khachhangRepo.UpdateAsync(khach, id);
                return RedirectToAction(nameof(Index));
            }

            var loaiKhachList = await _khachhangRepo.GetAllLoaikhach();
            ViewData["LoaiKhachId"] = new SelectList(loaiKhachList, "Maloaikhach", "Tenloaikhach", khach.Maloaikhach);
            return View(khach);
        }

        // 6. Xóa Khách Hàng (Tác vụ xóa trực tiếp)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _khachhangRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
