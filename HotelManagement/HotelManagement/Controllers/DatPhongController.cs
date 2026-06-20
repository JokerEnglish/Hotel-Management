using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelManagement.Controllers
{
    [Authorize]
    public class DatPhongController : Controller
    {
        private readonly IDatPhongRepository _datPhongRepo;
        private readonly IPhongRepository _phongRepo;
        private readonly IKhachhangRepository _khachhangRepo;
        private readonly IRentRepository _rentRepo;

        public DatPhongController(IDatPhongRepository datPhongRepo, IPhongRepository phongRepo, IKhachhangRepository khachhangRepo, IRentRepository rentRepo)
        {
            _datPhongRepo = datPhongRepo;
            _phongRepo = phongRepo;
            _khachhangRepo = khachhangRepo;
            _rentRepo = rentRepo;
        }

        // Dành cho Nhân viên/Admin xem tất cả yêu cầu đặt phòng
        [Authorize(Roles = "ADMIN,STAFF,NHANVIEN")] // Tuỳ theo role cấu hình
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Customer")) return RedirectToAction("AccessDenied", "Account");
            var danhSach = await _datPhongRepo.GetAllAsync();
            return View(danhSach);
        }

        // Lịch sử đặt phòng cá nhân (Dành cho Customer)
        public async Task<IActionResult> MyBookings()
        {
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId")?.Value;
            if (int.TryParse(customerIdClaim, out int customerId))
            {
                var danhSach = await _datPhongRepo.GetByCustomerIdAsync(customerId);
                return View(danhSach);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Book(int roomId)
        {
            var phong = await _phongRepo.GetByIdAsync(roomId);
            if (phong == null || phong.Tinhtrang != 1) 
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại hoặc đã được thuê.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Phong = phong;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(int Map, DateTime NgayNhan, DateTime NgayTra)
        {
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId")?.Value;
            if (!int.TryParse(customerIdClaim, out int customerId))
            {
                return RedirectToAction("Login", "Account");
            }

            var phong = await _phongRepo.GetByIdAsync(Map);
            if (phong == null || phong.Tinhtrang != 1)
            {
                TempData["ErrorMessage"] = "Phòng không khả dụng.";
                return RedirectToAction("Index", "Home");
            }

            if (NgayNhan < DateTime.Today || NgayTra <= NgayNhan)
            {
                TempData["ErrorMessage"] = "Ngày nhận/trả phòng không hợp lệ.";
                ViewBag.Phong = phong;
                return View();
            }

            // Tạm tính giá
            int days = (NgayTra - NgayNhan).Days;
            if (days == 0) days = 1;
            // Hiện tại CSDL chưa có cột giá phòng, nên tạm tính 500,000đ/ngày
            int tongTienDuKien = days * 500000;

            var datPhong = new DatPhong
            {
                Makh = customerId,
                Map = Map,
                NgayDat = DateTime.Now,
                NgayNhan = NgayNhan,
                NgayTra = NgayTra,
                TongTienDuKien = tongTienDuKien,
                Trangthai = 0 // Chờ duyệt
            };

            await _datPhongRepo.AddAsync(datPhong);
            
            // Có thể đổi tình trạng phòng thành 3 (Đã đặt) nếu hệ thống hỗ trợ
            // Nhưng hiện tại theo chuẩn thì cứ để Trống cho đến khi Duyệt thì đổi thành Đang thuê (2).
            // Tạm thời set thành 3 (Đã đặt) để người khác khỏi đặt trùng
            phong.Tinhtrang = 3; 
            await _phongRepo.UpdateAsync(phong);

            TempData["SuccessMessage"] = "Đặt phòng thành công. Vui lòng chờ nhân viên liên hệ xác nhận!";
            return RedirectToAction("MyBookings");
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            if (User.IsInRole("Customer")) return RedirectToAction("AccessDenied", "Account");

            var datPhong = await _datPhongRepo.GetByIdAsync(id);
            if (datPhong == null || datPhong.Trangthai != 0) return NotFound();

            var khach = datPhong.MakhNavigation;
            var phong = datPhong.MapNavigation;

            // Tạo phiếu thuê
            var phieuThue = new Phieuthue
            {
                Ngaylappt = DateTime.Now,
                Makh = datPhong.Makh,
                Map = datPhong.Map,
                Cccd = khach.Cmndkh
            };

            await _rentRepo.AddAsync(phieuThue);

            // Cập nhật tình trạng phòng -> 2 (Đang thuê)
            phong.Tinhtrang = 2;
            await _phongRepo.UpdateAsync(phong);

            // Cập nhật trạng thái đặt phòng -> 1 (Đã duyệt)
            datPhong.Trangthai = 1;
            await _datPhongRepo.UpdateAsync(datPhong);

            // Đưa khách vào phòng nếu chưa ở phòng nào
            if (khach.Map != phong.Map)
            {
                khach.Map = phong.Map;
                await _khachhangRepo.UpdateAsync(khach, khach.Makh);
            }

            TempData["SuccessMessage"] = "Đã duyệt đơn và lập Phiếu Thuê thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var datPhong = await _datPhongRepo.GetByIdAsync(id);
            if (datPhong == null || datPhong.Trangthai != 0) return NotFound();

            datPhong.Trangthai = 2; // Đã hủy
            await _datPhongRepo.UpdateAsync(datPhong);

            // Trả lại phòng về trạng thái trống (1)
            var phong = datPhong.MapNavigation;
            if (phong != null)
            {
                phong.Tinhtrang = 1;
                await _phongRepo.UpdateAsync(phong);
            }

            // Nếu là admin thao tác
            if (!User.IsInRole("Customer"))
            {
                TempData["SuccessMessage"] = "Đã từ chối/hủy đơn đặt phòng.";
                return RedirectToAction("Index");
            }
            
            // Nếu là khách hàng tự hủy
            TempData["SuccessMessage"] = "Đã hủy đơn đặt phòng.";
            return RedirectToAction("MyBookings");
        }
    }
}
