using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly INhanvienRepository _nvRepo;
        private readonly ITaikhoanRepository _taikhoanRepo;

        public AdminController(INhanvienRepository nvRepo, ITaikhoanRepository taikhoanRepo)
        {
            _nvRepo = nvRepo;
            _taikhoanRepo = taikhoanRepo;
        }

        public async Task<IActionResult> EmployeeList(string searchString)
        {
            var nv = await _nvRepo.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                nv = nv.Where(n => (n.Hoten != null && n.Hoten.ToLower().Contains(searchLower)) ||
                                   (n.Sdt != null && n.Sdt.Contains(searchString)) ||
                                   (n.Email != null && n.Email.ToLower().Contains(searchLower)));
            }
            ViewData["CurrentFilter"] = searchString;
            return View(nv);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Nhanvien nhanvien)
        {
            if (ModelState.IsValid)
            {
                await _nvRepo.AddAsync(nhanvien);
                return RedirectToAction(nameof(EmployeeList));
            }
            return View(nhanvien);
        }

        // GET: Admin/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var nhanvien = await _nvRepo.GetByIdAsync(id);
            if (nhanvien == null)
            {
                return NotFound();
            }
            return View(nhanvien);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Nhanvien nhanvien)
        {
            if (id != nhanvien.Manv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _nvRepo.UpdateAsync(nhanvien);
                return RedirectToAction(nameof(EmployeeList));
            }
            return View(nhanvien);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _nvRepo.DeleteAsync(id);
            return RedirectToAction(nameof(EmployeeList));
        }

        // GET: Hiển thị giao diện cấp tài khoản
        [HttpGet]
        public async Task<IActionResult> GrantAccount()
        {
            // Lấy danh sách nhân viên để hiển thị lên Dropdown
            var employees = await _nvRepo.GetAllAsync();
            ViewBag.Employees = employees;
            return View();
        }
        // POST: Nhận dữ liệu từ form gửi lên và lưu vào DB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GrantAccount(Taikhoan taikhoan)
        {
            // Bổ sung dòng này để bỏ qua lỗi validate thuộc tính liên kết Nhanvien
            ModelState.Remove("ManvNavigation");
            if (ModelState.IsValid)
            {
                await _taikhoanRepo.AddAsync(taikhoan);
                TempData["SuccessMessage"] = "Cấp tài khoản thành công!";
                return RedirectToAction(nameof(GrantAccount));
            }

            // Nếu có lỗi, load lại danh sách nhân viên cho Dropdown
            var employees = await _nvRepo.GetAllAsync();
            ViewBag.Employees = employees;
            return View(taikhoan);
        }

    }
}
