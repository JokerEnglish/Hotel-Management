using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagement.Controllers
{
    [Authorize]
    public class RentController : Controller
    {
        private readonly IRentRepository _rentRepo;
        private readonly IKhachhangRepository _khachRepo;
        private readonly IPhongRepository _phongRepo;

        public RentController(IRentRepository rentRepo, IKhachhangRepository khachRepo, IPhongRepository phongRepo)
        {
            _rentRepo = rentRepo;
            _khachRepo = khachRepo;
            _phongRepo = phongRepo;
        }

        // 1. Danh sách phiếu thuê
        public async Task<IActionResult> Index()
        {
            var rentList = await _rentRepo.GetAllAsync();
            
            if (User.IsInRole("Customer"))
            {
                var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId")?.Value;
                if (int.TryParse(customerIdClaim, out int customerId))
                {
                    rentList = rentList.Where(r => r.Makh == customerId).ToList();
                }
            }
            
            return View(rentList);
        }

        // 2. Tạo phiếu thuê (GET) - Chỉ hiện phòng TRỐNG (Tinhtrang == 1)
        [HttpGet]
        public async Task<IActionResult> Create(string roomName = null)
        {
            if (User.IsInRole("Customer")) return RedirectToAction("AccessDenied", "Account");

            // Lấy danh sách phòng TRỐNG, kèm tên loại phòng
            var phongsAvailable = await _phongRepo.GetRoomsByTinhtrangAsync(1);

            // Tạo dropdown hiển thị "Tên Phòng - Loại Phòng"
            var phongSelectList = phongsAvailable.Select(p => new SelectListItem
            {
                Value = p.Tenphong,
                Text = $"{p.Tenphong} ({p.MaloaiphongNavigation?.Tenloai ?? "Chưa rõ loại"})"
            }).ToList();

            ViewBag.DanhSachPhong = phongSelectList;
            ViewBag.SelectedRoom = roomName;
            return View();
        }

        // 3. Tạo phiếu thuê (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime ngayLapPt, string cccd, string tenPhong)
        {
            if (User.IsInRole("Customer")) return RedirectToAction("AccessDenied", "Account");

            // Kiểm tra CCCD khách hàng có tồn tại không
            var khach = await _khachRepo.GetClientByCCCDAsync(cccd);
            if (khach == null)
            {
                ModelState.AddModelError("cccd", "CCCD không hợp lệ hoặc chưa có trong hệ thống.");

                var phongsAvailable = await _phongRepo.GetRoomsByTinhtrangAsync(1);
                ViewBag.DanhSachPhong = phongsAvailable.Select(p => new SelectListItem
                {
                    Value = p.Tenphong,
                    Text = $"{p.Tenphong} ({p.MaloaiphongNavigation?.Tenloai ?? "Chưa rõ loại"})"
                }).ToList();
                ViewBag.ErrorCCCD = "❌ CCCD không hợp lệ. Vui lòng kiểm tra lại!";
                return View();
            }

            // Lấy thông tin phòng theo tên
            var phong = await _phongRepo.GetRoomByNameAsync(tenPhong);
            if (phong == null)
            {
                ViewBag.ErrorPhong = "❌ Phòng không tồn tại.";
                return View();
            }

            // Tạo phiếu thuê
            var phieuThue = new Phieuthue
            {
                Ngaylappt = ngayLapPt,
                Makh = khach.Makh,
                Map = phong.Map,
                Cccd = cccd
            };
            await _rentRepo.AddAsync(phieuThue);

            // Cập nhật tình trạng phòng → Đang thuê (2)
            phong.Tinhtrang = 2;
            await _phongRepo.UpdateAsync(phong);

            // Thêm khách vào phòng nếu chưa có
            if (khach.Map != phong.Map)
            {
                khach.Map = phong.Map;
                await _khachRepo.UpdateAsync(khach, khach.Makh);
            }

            return RedirectToAction(nameof(Index));
        }

        // 4. Xóa phiếu thuê
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            var rent = await _rentRepo.GetRentAsync(id);
            if (rent != null)
            {
                // Cập nhật tình trạng phòng → Trống (1)
                var phong = await _phongRepo.GetByIdAsync(rent.Map);
                if (phong != null)
                {
                    phong.Tinhtrang = 1;
                    await _phongRepo.UpdateAsync(phong);
                }
                await _rentRepo.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

        // 5. Sửa phiếu thuê (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (User.IsInRole("Customer")) return RedirectToAction("AccessDenied", "Account");

            var rent = await _rentRepo.GetRentAsync(id);
            if (rent == null) return NotFound();

            // Lấy danh sách phòng TRỐNG + cả phòng đang được chọn trong phiếu thuê này
            var phongsAvailable = await _phongRepo.GetRoomsByTinhtrangAsync(1);
            var currentRoom = await _phongRepo.GetByIdAsync(rent.Map);
            if (currentRoom != null && currentRoom.Tinhtrang != 1)
            {
                phongsAvailable.Add(currentRoom);
            }

            ViewBag.DanhSachPhong = phongsAvailable.Select(p => new SelectListItem
            {
                Value = p.Tenphong,
                Text = $"{p.Tenphong} ({p.MaloaiphongNavigation?.Tenloai ?? "Chưa rõ loại"})"
            }).ToList();

            ViewBag.CurrentTenPhong = currentRoom?.Tenphong;

            return View(rent);
        }

        // 6. Sửa phiếu thuê (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DateTime Ngaylappt, string Cccd, string TenPhong)
        {
            if (User.IsInRole("Customer")) return RedirectToAction("AccessDenied", "Account");

            var rent = await _rentRepo.GetRentAsync(id);
            if (rent == null) return NotFound();

            // Kiểm tra CCCD
            var khach = await _khachRepo.GetClientByCCCDAsync(Cccd);
            if (khach == null)
            {
                ModelState.AddModelError("Cccd", "CCCD không hợp lệ hoặc chưa có trong hệ thống.");
                // Load lại view
                var phongs = await _phongRepo.GetRoomsByTinhtrangAsync(1);
                var currentR = await _phongRepo.GetByIdAsync(rent.Map);
                if (currentR != null) phongs.Add(currentR);
                ViewBag.DanhSachPhong = phongs.Select(p => new SelectListItem { Value = p.Tenphong, Text = p.Tenphong }).ToList();
                return View(rent);
            }

            var newPhong = await _phongRepo.GetRoomByNameAsync(TenPhong);
            if (newPhong == null) return NotFound();

            // Xử lý đổi phòng
            if (rent.Map != newPhong.Map)
            {
                // Phòng cũ -> Trống (1)
                var oldPhong = await _phongRepo.GetByIdAsync(rent.Map);
                if (oldPhong != null)
                {
                    oldPhong.Tinhtrang = 1;
                    await _phongRepo.UpdateAsync(oldPhong);
                }

                // Phòng mới -> Đang thuê (2)
                newPhong.Tinhtrang = 2;
                await _phongRepo.UpdateAsync(newPhong);

                // Cập nhật khách hàng sang phòng mới
                khach.Map = newPhong.Map;
                await _khachRepo.UpdateAsync(khach, khach.Makh);
            }

            // Cập nhật phiếu thuê
            rent.Ngaylappt = Ngaylappt;
            rent.Makh = khach.Makh;
            rent.Cccd = Cccd;
            rent.Map = newPhong.Map;

            await _rentRepo.UpdateAsync(rent);
            return RedirectToAction(nameof(Index));
        }
        // 7. Xem chi tiết phiếu thuê (GET)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var rent = await _rentRepo.GetRentAsync(id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }
    }
}
