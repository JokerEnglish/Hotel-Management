# 🏨 Đồ án: Hệ thống Quản lý Khách sạn
## Môn: Lập trình Web

---

## 1. Tổng quan Đề tài

Xây dựng hệ thống **Quản lý Khách sạn** hoàn chỉnh gồm:
- **Database** chuyên sâu với SQL Server (stored procedures, triggers, views, transactions, indexing)
- **Web Application** với **ASP.NET Core MVC** (.NET 8) — gồm cả Backend lẫn Frontend trong 1 project

> [!IMPORTANT]
> Đây là môn **Lập trình Web**, sử dụng mô hình **MVC (Model-View-Controller)** của ASP.NET Core. Giao diện dùng **Razor Views** kết hợp **Bootstrap 5** + **JavaScript** để tạo trải nghiệm người dùng mượt mà. Database dùng **SQL Server** với các kỹ thuật nâng cao.

---

## 2. Công nghệ & Công cụ Sử dụng

### 2.1. Stack công nghệ

```
┌─────────────────────────────────────────┐
│   ASP.NET Core MVC (.NET 8)             │
│   ┌───────────┬───────────┬───────────┐ │
│   │  Models   │   Views   │Controllers│ │
│   │  (C#)     │  (Razor)  │   (C#)    │ │
│   └───────────┴───────────┴───────────┘ │
│         ↕ Dapper (Micro ORM)            │
├─────────────────────────────────────────┤
│          DATABASE (SQL Server)          │
└─────────────────────────────────────────┘
```

### 2.2. Bảng công cụ chi tiết

| Hạng mục | Công cụ | Phiên bản | Ghi chú |
|----------|---------|-----------|---------|
| **IDE chính** | Visual Studio 2022 (Community) | Latest | Workload: *ASP.NET and web development* |
| **DBMS** | SQL Server | 2022 / Express | [microsoft.com/sql-server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) |
| **SQL Client** | SSMS 22 | Latest | Quản lý database, chạy SQL scripts |
| **ERD Design** | dbdiagram.io | Online | [dbdiagram.io](https://dbdiagram.io) — Vẽ ERD, xuất ảnh |
| **Backend Runtime** | .NET SDK | 8.0 LTS | [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0) |
| **Web Framework** | ASP.NET Core MVC | .NET 8 | Model-View-Controller, Razor Views |
| **ORM / DB Access** | Dapper | Latest | Gọi Stored Procedures từ C# — đơn giản, linh hoạt |
| **DB Driver** | Microsoft.Data.SqlClient | Latest | Kết nối SQL Server từ .NET |
| **UI Framework** | Bootstrap 5 | 5.3 | Responsive layout, components đẹp sẵn |
| **Icons** | Bootstrap Icons | Latest | Bộ icon miễn phí |
| **Biểu đồ** | Chart.js | Latest | Dashboard doanh thu, công suất phòng |
| **JavaScript** | jQuery (tùy chọn) | 3.x | AJAX, DOM manipulation |
| **Version Control** | Git + GitHub | — | Quản lý source code |
| **Báo cáo** | Google Docs | — | Viết tài liệu đồ án |

### 2.3. Visual Studio 2022 — Cài đặt & Extensions

**Workloads cần chọn khi cài Visual Studio 2022:**
```
✅ ASP.NET and web development
✅ Data storage and processing
```

**Extensions khuyến nghị:**
```
- GitHub Extension for Visual Studio   → Kết nối GitHub
- SQL Server Data Tools (SSDT)         → Tích hợp SQL Server trong VS
- Bundler & Minifier 2022+             → Tối ưu file CSS/JS
```

**Phím tắt hữu ích:**
```
- Ctrl+Shift+B    → Build Solution
- F5              → Chạy Debug
- Ctrl+F5         → Chạy không Debug
- Ctrl+.          → Auto-fix lỗi, thêm using
- F12             → Go To Definition
```

---

## 3. Cấu trúc Thư mục Dự án

### 3.1. Cấu trúc Solution (Visual Studio 2022)

```
HotelManagement.sln                          ← Solution
└── HotelManagement/                         ← 1 project MVC duy nhất
    ├── Controllers/                         ← Xử lý logic
    │   ├── HomeController.cs                → Dashboard
    │   ├── PhongController.cs               → Quản lý phòng
    │   ├── DatPhongController.cs            → Đặt phòng, check-in/out
    │   ├── KhachHangController.cs           → Quản lý khách hàng
    │   ├── DichVuController.cs              → Quản lý dịch vụ
    │   ├── HoaDonController.cs              → Hóa đơn, thanh toán
    │   ├── NhanVienController.cs            → Quản lý nhân viên
    │   ├── BaoCaoController.cs              → Báo cáo doanh thu
    │   └── AccountController.cs             → Đăng nhập, phân quyền
    │
    ├── Models/                              ← Entities + ViewModels
    │   ├── Entities/
    │   │   ├── LoaiPhong.cs
    │   │   ├── Phong.cs
    │   │   ├── KhachHang.cs
    │   │   ├── DatPhong.cs
    │   │   ├── ChiTietDatPhong.cs
    │   │   ├── DichVu.cs
    │   │   ├── SuDungDichVu.cs
    │   │   ├── HoaDon.cs
    │   │   ├── NhanVien.cs
    │   │   └── LichLamViec.cs
    │   └── ViewModels/
    │       ├── DashboardViewModel.cs
    │       ├── DatPhongViewModel.cs
    │       ├── CheckOutViewModel.cs
    │       └── BaoCaoViewModel.cs
    │
    ├── Repositories/                        ← Data Access (Dapper)
    │   ├── Interfaces/
    │   │   ├── IPhongRepository.cs
    │   │   ├── IDatPhongRepository.cs
    │   │   ├── IKhachHangRepository.cs
    │   │   └── IBaoCaoRepository.cs
    │   └── Implementations/
    │       ├── PhongRepository.cs
    │       ├── DatPhongRepository.cs
    │       ├── KhachHangRepository.cs
    │       └── BaoCaoRepository.cs
    │
    ├── Views/                               ← Giao diện Razor (.cshtml)
    │   ├── Shared/
    │   │   ├── _Layout.cshtml               → Layout chung (sidebar, navbar)
    │   │   ├── _LoginLayout.cshtml          → Layout trang đăng nhập
    │   │   └── _ValidationScriptsPartial.cshtml
    │   ├── Home/
    │   │   └── Index.cshtml                 → Dashboard
    │   ├── Phong/
    │   │   ├── Index.cshtml                 → Danh sách phòng
    │   │   ├── SoDo.cshtml                  → Sơ đồ phòng trực quan
    │   │   ├── Create.cshtml                → Thêm phòng
    │   │   └── Edit.cshtml                  → Sửa phòng
    │   ├── DatPhong/
    │   │   ├── Index.cshtml                 → Danh sách booking
    │   │   ├── Create.cshtml                → Tạo đặt phòng mới
    │   │   ├── CheckIn.cshtml               → Màn hình check-in
    │   │   └── CheckOut.cshtml              → Màn hình check-out + hóa đơn
    │   ├── KhachHang/
    │   │   ├── Index.cshtml                 → Danh sách khách hàng
    │   │   └── Details.cshtml               → Chi tiết + lịch sử lưu trú
    │   ├── DichVu/
    │   │   └── Index.cshtml                 → Quản lý dịch vụ
    │   ├── NhanVien/
    │   │   ├── Index.cshtml                 → Danh sách nhân viên
    │   │   └── LichLamViec.cshtml           → Lịch làm việc
    │   ├── BaoCao/
    │   │   └── Index.cshtml                 → Báo cáo doanh thu + biểu đồ
    │   └── Account/
    │       └── Login.cshtml                 → Trang đăng nhập
    │
    ├── wwwroot/                             ← File tĩnh (CSS, JS, Images)
    │   ├── css/
    │   │   └── site.css                     → CSS tùy chỉnh
    │   ├── js/
    │   │   └── site.js                      → JavaScript tùy chỉnh
    │   ├── lib/
    │   │   ├── bootstrap/                   → Bootstrap 5
    │   │   └── chart.js/                    → Chart.js (biểu đồ)
    │   └── images/                          → Hình ảnh
    │
    ├── Program.cs                           → Cấu hình DI, Auth, Middleware
    ├── appsettings.json                     → ConnectionString SQL Server
    └── HotelManagement.csproj               → NuGet packages
```

### 3.2. Database Scripts (chạy trên SSMS 22)

```
database/
├── 01_schema.sql                → Tạo 10 bảng, constraints
├── 02_indexes.sql               → Tạo index
├── 03_views.sql                 → 6 views
├── 04_stored_procedures.sql     → 6 stored procedures
├── 05_triggers.sql              → 5 triggers
├── 06_seed_data.sql             → Dữ liệu mẫu
└── 07_roles_permissions.sql     → Phân quyền
```

---

## 4. Phân tích Nghiệp vụ

### 4.1. Các Actor
| Actor | Vai trò |
|-------|---------|
| **Quản lý** | Xem báo cáo doanh thu, quản lý tổng thể khách sạn |
| **Nhân viên Lễ tân** | Đặt phòng, check-in, check-out, lập hóa đơn |
| **Nhân viên Phục vụ** | Ghi nhận dịch vụ phát sinh (ăn uống, spa, dọn phòng...) |
| **Khách hàng** | Đặt phòng online, xem lịch sử và điểm tích lũy |
| **Kế toán** | Quản lý hóa đơn, thanh toán |

### 4.2. Chức năng chính
- ✅ Quản lý phòng & loại phòng
- ✅ Đặt phòng (Booking) — trực tiếp & online
- ✅ Check-in / Check-out
- ✅ Quản lý dịch vụ bổ sung (ăn uống, spa, giặt ủi...)
- ✅ Quản lý khách hàng & điểm tích lũy
- ✅ Quản lý hóa đơn & thanh toán
- ✅ Báo cáo doanh thu, công suất phòng

---

## 5. Thiết kế Cơ sở Dữ liệu

### 5.1. Các bảng chính (10 bảng)

```sql
-- 1. Loại phòng
LoaiPhong (MaLoaiPhong, TenLoai, MoTa, GiaMoiDem, SucChuaToiDa, TienNghi, HinhAnh)

-- 2. Phòng
Phong (MaPhong, SoPhong, Tang, MaLoaiPhong, TrangThai, MoTa, HinhAnh)
-- TrangThai: 'TRONG', 'DA_DAT', 'DANG_O', 'BAO_TRI'

-- 3. Khách hàng
KhachHang (MaKH, HoTen, CCCD, DiaChi, SDT, Email, NgaySinh, LoaiKH, DiemTichLuy)
-- LoaiKH: 'THUONG', 'THAN_THIET', 'VIP'

-- 4. Đặt phòng
DatPhong (MaDatPhong, MaKH, NgayDat, NgayDen, NgayDi, SoNguoiLon, SoTreEm,
          TrangThai, TongTien, GhiChu, KenhDat, NhanVienXuLy)
-- TrangThai: 'CHO_XAC_NHAN', 'DA_XAC_NHAN', 'DA_CHECKIN', 'DA_CHECKOUT', 'HUY'

-- 5. Chi tiết đặt phòng
ChiTietDatPhong (MaChiTiet, MaDatPhong, MaPhong, GiaThucTe, GhiChu)

-- 6. Dịch vụ
DichVu (MaDichVu, TenDichVu, MoTa, DonGia, DonViTinh, LoaiDichVu, TrangThai)

-- 7. Sử dụng dịch vụ
SuDungDichVu (MaSuDung, MaDatPhong, MaDichVu, SoLuong, DonGia, ThoiGian, GhiChu)

-- 8. Hóa đơn
HoaDon (MaHoaDon, MaDatPhong, NgayLap, TienPhong, TienDichVu,
        GiamGia, ThueVAT, TongCong, PhuongThucTT, TrangThaiTT, NhanVienThu)

-- 9. Nhân viên
NhanVien (MaNV, HoTen, CCCD, LoaiNV, PhongBan, SDT, Email, LuongCoBan, NgayVaoLam, TrangThai)
-- LoaiNV: 'LE_TAN' | 'PHUC_VU' | 'QUAN_LY' | 'KE_TOAN'

-- 10. Lịch làm việc
LichLamViec (MaLich, MaNV, NgayLam, CaLam, GioVao, GioRa, GhiChu)
-- CaLam: 'SANG' (6h-14h) | 'CHIEU' (14h-22h) | 'DEM' (22h-6h)
```

### 5.2. Sơ đồ ERD (tổng quan)
```
KhachHang ──< DatPhong >── ChiTietDatPhong >── Phong
                  │                                 │
                  │                             LoaiPhong
                  │
              SuDungDichVu >── DichVu
                  │
               HoaDon
                   
NhanVien ──< LichLamViec
```

---

## 6. Kỹ thuật CSDL Nâng cao (Bổ trợ Web)

### 6.1. Stored Procedures
```sql
sp_DatPhong(MaKH, NgayDen, NgayDi, MaLoaiPhong, SoNguoi)
sp_CheckIn(MaDatPhong, NhanVienXuLy)
sp_CheckOut(MaDatPhong, PhuongThucThanhToan)
sp_TimPhongTrong(NgayDen, NgayDi, MaLoaiPhong, SoNguoi)
sp_BaoCaoDoanhThu(Thang, Nam)
sp_CapNhatDiemTichLuy(MaKH, SoTien)
```

### 6.2. Triggers
```sql
trg_CapNhatTrangThaiPhong     -- Tự động cập nhật phòng khi check-in/out
trg_TinhTongTienHoaDon        -- Tự động tính lại tổng tiền khi thêm DV
trg_LogThayDoiGiaPhong        -- Ghi log khi thay đổi giá
trg_KiemTraDatPhongTrung      -- Cảnh báo đặt phòng trùng
trg_CapNhatDiemSauCheckout    -- Cộng điểm tích lũy sau checkout
```

### 6.3. Views
```sql
v_PhongTrong                  -- Phòng trống hiện tại
v_KhachDangO                  -- Danh sách khách đang lưu trú
v_DoanhThuTheoLoaiPhong       -- Doanh thu theo từng loại phòng
v_LichSuLuuTru                -- Lịch sử lưu trú của khách hàng
v_CongSuatPhong               -- Công suất phòng theo tháng
v_KhachHangVIP                -- Khách hàng VIP với điểm tích lũy cao nhất
```

---

## 7. Cấu hình ASP.NET Core MVC

### 7.1. Khởi tạo Project trong Visual Studio 2022

```
Bước 1: File → New → Project
Bước 2: Chọn "ASP.NET Core Web App (Model-View-Controller)"
Bước 3: Điền thông tin:
   - Project name: HotelManagement
   - Location: C:\Visual tim (hoặc thư mục bạn chọn)
   - Solution name: HotelManagement
Bước 4: Next → Chọn:
   - Framework: .NET 8.0
   - Authentication type: None (tự làm)
   - ✅ Configure for HTTPS
   - ✅ Do not use top-level statements
```

### 7.2. NuGet Packages cần cài

> **Tools → NuGet Package Manager → Package Manager Console**

```powershell
Install-Package Dapper
Install-Package Microsoft.Data.SqlClient
Install-Package Microsoft.AspNetCore.Authentication.Cookies
```

### 7.3. Cấu hình appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=HotelManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 7.4. Cấu hình Program.cs

```csharp
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC
builder.Services.AddControllersWithViews();

// 2. Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 3. Đăng ký Repositories (Dependency Injection)
builder.Services.AddScoped<IPhongRepository>(sp =>
    new PhongRepository(connectionString!));
builder.Services.AddScoped<IDatPhongRepository>(sp =>
    new DatPhongRepository(connectionString!));
builder.Services.AddScoped<IKhachHangRepository>(sp =>
    new KhachHangRepository(connectionString!));
// ... thêm các repository khác

// 4. Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### 7.5. Ví dụ Repository (Dapper gọi Stored Procedure)

```csharp
using Dapper;
using Microsoft.Data.SqlClient;

namespace HotelManagement.Repositories;

public class DatPhongRepository : IDatPhongRepository
{
    private readonly string _connectionString;

    public DatPhongRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> DatPhongAsync(DatPhong datPhong)
    {
        using var conn = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@MaKH", datPhong.MaKH);
        parameters.Add("@NgayDen", datPhong.NgayDen);
        parameters.Add("@NgayDi", datPhong.NgayDi);
        parameters.Add("@MaLoaiPhong", datPhong.MaLoaiPhong);
        parameters.Add("@SoNguoi", datPhong.SoNguoiLon);
        parameters.Add("@MaDatPhong", dbType: System.Data.DbType.Int32,
                        direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync("sp_DatPhong", parameters,
                                commandType: System.Data.CommandType.StoredProcedure);
        return parameters.Get<int>("@MaDatPhong");
    }
}
```

### 7.6. Ví dụ Controller

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers;

[Authorize]
public class DatPhongController : Controller
{
    private readonly IDatPhongRepository _repo;
    private readonly IPhongRepository _phongRepo;

    public DatPhongController(IDatPhongRepository repo, IPhongRepository phongRepo)
    {
        _repo = repo;
        _phongRepo = phongRepo;
    }

    // GET: /DatPhong
    public async Task<IActionResult> Index()
    {
        var bookings = await _repo.GetAllAsync();
        return View(bookings);
    }

    // GET: /DatPhong/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.PhongTrong = await _phongRepo.GetPhongTrongAsync();
        return View();
    }

    // POST: /DatPhong/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DatPhong datPhong)
    {
        if (ModelState.IsValid)
        {
            await _repo.DatPhongAsync(datPhong);
            return RedirectToAction(nameof(Index));
        }
        return View(datPhong);
    }

    // POST: /DatPhong/CheckIn/5
    [HttpPost]
    public async Task<IActionResult> CheckIn(int id)
    {
        await _repo.CheckInAsync(id, User.Identity.Name);
        return RedirectToAction(nameof(Index));
    }
}
```

### 7.7. Ví dụ Razor View (Danh sách Booking)

```html
@model IEnumerable<DatPhong>

<div class="container-fluid">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2><i class="bi bi-calendar-check"></i> Danh sách Đặt phòng</h2>
        <a asp-action="Create" class="btn btn-primary">
            <i class="bi bi-plus-lg"></i> Đặt phòng mới
        </a>
    </div>

    <div class="card shadow-sm">
        <div class="card-body">
            <table class="table table-hover">
                <thead class="table-dark">
                    <tr>
                        <th>Mã</th>
                        <th>Khách hàng</th>
                        <th>Ngày đến</th>
                        <th>Ngày đi</th>
                        <th>Trạng thái</th>
                        <th>Thao tác</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var item in Model)
                    {
                        <tr>
                            <td>@item.MaDatPhong</td>
                            <td>@item.TenKhachHang</td>
                            <td>@item.NgayDen.ToString("dd/MM/yyyy")</td>
                            <td>@item.NgayDi.ToString("dd/MM/yyyy")</td>
                            <td>
                                <span class="badge bg-@item.BadgeColor">
                                    @item.TrangThai
                                </span>
                            </td>
                            <td>
                                <a asp-action="CheckIn" asp-route-id="@item.MaDatPhong"
                                   class="btn btn-sm btn-success">Check-in</a>
                                <a asp-action="CheckOut" asp-route-id="@item.MaDatPhong"
                                   class="btn btn-sm btn-warning">Check-out</a>
                            </td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
    </div>
</div>
```

---

## 8. Giao diện (Razor Views + Bootstrap 5)

### 8.1. Các màn hình chính

| Màn hình | Controller/Action | Chức năng |
|----------|-------------------|-----------|
| **Đăng nhập** | Account/Login | Đăng nhập theo vai trò |
| **Dashboard** | Home/Index | Thống kê doanh thu, công suất, khách đang ở |
| **Quản lý Phòng** | Phong/Index | Danh sách phòng, thêm/sửa/xóa |
| **Sơ đồ Phòng** | Phong/SoDo | Hiển thị phòng theo màu trạng thái |
| **Đặt Phòng** | DatPhong/Create | Tìm phòng trống, tạo booking |
| **Danh sách Booking** | DatPhong/Index | Xem & duyệt đặt phòng, check-in |
| **Check-out** | DatPhong/CheckOut | Dịch vụ phát sinh, xuất hóa đơn |
| **Khách hàng** | KhachHang/Index | Danh sách, điểm tích lũy, lịch sử |
| **Dịch vụ** | DichVu/Index | Quản lý danh mục dịch vụ |
| **Báo cáo** | BaoCao/Index | Doanh thu theo tháng (Chart.js) |
| **Nhân viên** | NhanVien/Index | Quản lý nhân viên |
| **Lịch làm việc** | NhanVien/LichLamViec | Phân ca sáng/chiều/đêm |

### 8.2. UI Highlights
- 🎨 **Bootstrap 5** — Responsive, giao diện hiện đại
- 🗺️ **Sơ đồ phòng trực quan** — Hiển thị trạng thái phòng theo màu sắc
- 📊 **Dashboard với biểu đồ** — Chart.js (doanh thu, công suất phòng)
- 🧾 **In hóa đơn** — Xuất hóa đơn khi checkout
- 🔐 **Phân quyền** — Hiển thị menu theo vai trò (Quản lý, Lễ tân, Phục vụ...)

---

## 9. Kế hoạch Thực hiện — 5 Tuần

### 📅 Tuần 1: Thiết kế Database + Khởi tạo Project
- [ ] Vẽ ERD trên dbdiagram.io & Chuẩn hóa
- [ ] Viết `01_schema.sql` — tạo 10 bảng, constraints
- [ ] Viết `02_indexes.sql`
- [ ] Viết `06_seed_data.sql` — dữ liệu mẫu
- [ ] Tạo project MVC trong Visual Studio 2022
- [ ] Cài NuGet packages: Dapper, SqlClient, Authentication.Cookies
- [ ] Cấu hình `appsettings.json` + `Program.cs`
- [ ] Tạo layout chung `_Layout.cshtml` (sidebar + navbar Bootstrap 5)

### 📅 Tuần 2: Database nâng cao + Models & Repositories
- [ ] Viết `03_views.sql` — 6 views
- [ ] Viết `04_stored_procedures.sql` — 6 SPs
- [ ] Viết `05_triggers.sql` — 5 triggers
- [ ] Viết `07_roles_permissions.sql`
- [ ] Viết Models/Entities (10 entities) + ViewModels
- [ ] Viết Repositories gọi Stored Procedures qua Dapper
- [ ] Tạo trang Login + Cookie Authentication

### 📅 Tuần 3: Controllers + Views — Chức năng chính
- [ ] Dashboard (Home/Index) — thống kê + biểu đồ Chart.js
- [ ] Quản lý Phòng — CRUD + Sơ đồ phòng trực quan
- [ ] Đặt phòng — tìm phòng trống, tạo booking
- [ ] Danh sách Booking — xem, duyệt, check-in

### 📅 Tuần 4: Views — Nghiệp vụ còn lại
- [ ] Check-out + Hóa đơn
- [ ] Quản lý Khách hàng — danh sách, điểm tích lũy, lịch sử
- [ ] Quản lý Dịch vụ
- [ ] Quản lý Nhân viên + Lịch làm việc
- [ ] Báo cáo doanh thu (Chart.js)

### 📅 Tuần 5: Hoàn thiện & Báo cáo
- [ ] Test tích hợp toàn bộ luồng (Book → CheckIn → CheckOut → Invoice)
- [ ] Fix bug, tối ưu UI/UX
- [ ] Phân quyền theo vai trò (Quản lý, Lễ tân, Phục vụ, Kế toán)
- [ ] Viết báo cáo đồ án (Word/Google Docs)
- [ ] Chuẩn bị slide thuyết trình + demo

---

## 10. Tiêu chí Chấm điểm (Web Programming)

| Tiêu chí | Trọng số | Nội dung |
|----------|----------|---------|
| Frontend UI/UX | 30% | Giao diện đẹp, responsive, trải nghiệm người dùng tốt |
| Backend API & Logic | 30% | MVC chuẩn, xử lý logic nghiệp vụ tốt |
| Database Design | 20% | Đúng quan hệ, chuẩn hóa, thiết kế bảng hợp lý |
| Tính năng hệ thống | 10% | Đầy đủ các chức năng quản lý chính |
| Báo cáo & Demo | 10% | Rõ ràng, demo live mượt mà |

---

## 11. Lưu ý Quan trọng

> [!IMPORTANT]
> - Luôn **Build Solution** (`Ctrl+Shift+B`) trước khi chạy để kiểm tra lỗi compile
> - **Connection String** phải khớp tên SQL Server instance (thường là `localhost\SQLEXPRESS`)
> - Mở **SSMS 22** để chạy các file SQL scripts tạo database trước khi chạy web

> [!TIP]
> - Dùng **`Ctrl+.`** để auto-fix lỗi và thêm `using` statements
> - Dùng **`F12`** để Go To Definition
> - Dùng **Tag Helpers** (`asp-for`, `asp-action`, `asp-controller`) thay vì viết HTML thủ công
> - Dùng **Partial Views** cho các component dùng lại nhiều lần

> [!WARNING]
> - Không commit file `appsettings.json` chứa password lên GitHub
> - Thêm `appsettings.json` vào `.gitignore` nếu có thông tin nhạy cảm
> - Dùng `appsettings.Development.json` cho môi trường local

---

## 12. Tài liệu Tham khảo

- 📖 [ASP.NET Core MVC — Docs](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview) — Tài liệu chính thức
- 📖 [.NET 8 Download](https://dotnet.microsoft.com/download/dotnet/8.0) — Cài .NET SDK
- 📖 [Dapper](https://github.com/DapperLib/Dapper) — Micro ORM gọi Stored Procedures từ C#
- 📖 [Visual Studio 2022 Download](https://visualstudio.microsoft.com/vs/) — IDE chính
- 📖 [SQL Server Express Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- 📖 [SSMS Download](https://aka.ms/ssmsfullsetup)
- 📖 [Bootstrap 5](https://getbootstrap.com/) — UI Framework
- 📖 [Bootstrap Icons](https://icons.getbootstrap.com/) — Bộ icon
- 📖 [Chart.js](https://www.chartjs.org/) — Biểu đồ JavaScript
- 📖 [Razor Views Syntax](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor) — Cú pháp Razor
