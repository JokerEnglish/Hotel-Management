# 🏨 Đồ án: Hệ thống Quản lý Khách sạn
## Môn: Lập trình Web

---

## 1. Tổng quan Đề tài

Xây dựng hệ thống **Quản lý Khách sạn** hoàn chỉnh gồm:
- **Database** chuyên sâu với SQL Server (stored procedures, triggers, views, transactions, indexing)
- **Backend API** với **ASP.NET Core Web API** (.NET 8) để kết nối DB và giao diện
- **Frontend** với React.js (Vite) để tương tác người dùng

> [!IMPORTANT]
> Đây là môn **Lập trình Web**, nên trọng tâm sẽ là sự kết hợp hoàn hảo giữa **Frontend (React)**, **Backend (ASP.NET Core Web API)** và **Database (SQL Server)**. Giao diện cần mượt mà, UX/UI tốt và API xử lý logic chặt chẽ.

---

## 2. Công nghệ & Công cụ Sử dụng

### 2.1. Stack công nghệ

```
┌─────────────────────────────────────────┐
│        FRONTEND (React.js + Vite)       │  ← Giao diện người dùng
├─────────────────────────────────────────┤
│     BACKEND (ASP.NET Core Web API)      │  ← REST API (.NET 8)
├─────────────────────────────────────────┤
│          DATABASE (SQL Server)          │  ← Trọng tâm đồ án
└─────────────────────────────────────────┘
```

### 2.2. Bảng công cụ chi tiết

| Hạng mục | Công cụ | Phiên bản | Ghi chú |
|----------|---------|-----------|---------|
| **IDE chính** | Visual Studio 2022 (Community) | Latest | Workload: *ASP.NET and web development* |
| **DBMS** | SQL Server | 2022 / Express | [microsoft.com/sql-server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) |
| **SQL Client** | SSMS hoặc DBeaver | Latest | [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) |
| **ERD Design** | dbdiagram.io | Online | [dbdiagram.io](https://dbdiagram.io) — Viết DBML, xuất ảnh |
| **Backend Runtime** | .NET SDK | 8.0 LTS | [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0) |
| **Backend Framework** | ASP.NET Core Web API | .NET 8 | REST API, Swagger tích hợp sẵn |
| **ORM / DB Access** | Dapper | Latest | Gọi Stored Procedures từ C# — đơn giản, linh hoạt |
| **DB Driver** | Microsoft.Data.SqlClient | Latest | Kết nối SQL Server từ .NET |
| **Auth** | ASP.NET Core JWT Bearer | .NET 8 | Xác thực token cho API |
| **Frontend Framework** | React.js + Vite | 18 + 5.x | Đơn giản, linh hoạt, dễ học |
| **UI Library** | Ant Design hoặc MUI | Latest | Component quản lý sẵn có (table, form, chart) |
| **Routing** | React Router DOM | v6 | Điều hướng giữa các trang |
| **HTTP Client** | Axios | Latest | Gọi REST API từ frontend |
| **Biểu đồ** | Recharts | Latest | Dashboard doanh thu, công suất |
| **API Test** | Swagger UI / Postman | Latest | Swagger tích hợp sẵn trong ASP.NET Core |
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
- Bundler & Minifier 2022+             → Tối ưu file tĩnh
```

**Phím tắt hữu ích:**
```
- Ctrl+Shift+B    → Build Solution (kiểm tra lỗi compile)
- F5              → Chạy Debug (mở Swagger tự động)
- Ctrl+F5         → Chạy không Debug
- Ctrl+.          → Auto-fix lỗi, thêm using
- F12             → Go To Definition
```

---

## 3. Cấu trúc Thư mục Dự án

### 3.1. Cấu trúc Solution (Visual Studio 2022)

```
HotelManagement.sln
├── HotelManagement.API              ← ASP.NET Core Web API (.NET 8)
├── HotelManagement.Models           ← Class Library: Entities, DTOs
└── HotelManagement.Repositories     ← Class Library: Data Access (Dapper)
```

### 3.2. Cấu trúc thư mục chi tiết

```
hotel-management/
├── database/                        ← TẤT CẢ SCRIPT SQL
│   ├── 01_schema.sql                → Tạo bảng, constraints
│   ├── 02_indexes.sql               → Tạo index
│   ├── 03_views.sql                 → Tạo views
│   ├── 04_stored_procedures.sql     → Stored procedures
│   ├── 05_triggers.sql              → Triggers
│   ├── 06_seed_data.sql             → Dữ liệu mẫu
│   └── 07_roles_permissions.sql     → Phân quyền
│
├── backend/                         ← ASP.NET CORE WEB API (.NET 8)
│   ├── HotelManagement.API/
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs       → POST /api/auth/login
│   │   │   ├── RoomsController.cs      → API phòng
│   │   │   ├── BookingsController.cs   → API đặt phòng
│   │   │   ├── CustomersController.cs  → API khách hàng
│   │   │   ├── ServicesController.cs   → API dịch vụ
│   │   │   ├── StaffController.cs      → API nhân viên
│   │   │   └── ReportsController.cs    → API báo cáo
│   │   ├── Program.cs                  → Cấu hình DI, Swagger, CORS, JWT
│   │   ├── appsettings.json            → ConnectionString SQL Server
│   │   └── HotelManagement.API.csproj
│   │
│   ├── HotelManagement.Models/
│   │   ├── Entities/                   → Entity classes (C#)
│   │   │   ├── LoaiPhong.cs
│   │   │   ├── Phong.cs
│   │   │   ├── KhachHang.cs
│   │   │   ├── DatPhong.cs
│   │   │   ├── ChiTietDatPhong.cs
│   │   │   ├── DichVu.cs
│   │   │   ├── SuDungDichVu.cs
│   │   │   ├── HoaDon.cs
│   │   │   ├── NhanVien.cs
│   │   │   └── LichLamViec.cs
│   │   └── DTOs/                       → Data Transfer Objects
│   │       ├── DatPhongDto.cs
│   │       ├── CheckInDto.cs
│   │       ├── CheckOutDto.cs
│   │       ├── BaoCaoDto.cs
│   │       └── AuthDto.cs
│   │
│   └── HotelManagement.Repositories/
│       ├── Interfaces/
│       │   ├── IPhongRepository.cs
│       │   ├── IDatPhongRepository.cs
│       │   ├── IKhachHangRepository.cs
│       │   └── IBaoCaoRepository.cs
│       └── Implementations/
│           ├── PhongRepository.cs
│           ├── DatPhongRepository.cs
│           ├── KhachHangRepository.cs
│           └── BaoCaoRepository.cs
│
├── frontend/                        ← REACT.JS (VITE) UI
│   ├── src/
│   │   ├── pages/
│   │   │   ├── Dashboard.jsx        → Trang tổng quan
│   │   │   ├── Rooms.jsx            → Quản lý phòng
│   │   │   ├── Bookings.jsx         → Quản lý đặt phòng
│   │   │   ├── Customers.jsx        → Quản lý khách hàng
│   │   │   ├── Staff.jsx            → Quản lý nhân viên
│   │   │   └── Reports.jsx          → Báo cáo doanh thu
│   │   ├── components/              → Các component dùng chung
│   │   ├── services/                → Gọi API (axios)
│   │   └── App.jsx
│   └── package.json
│
└── docs/                            ← TÀI LIỆU
    ├── ERD.png
    └── BaoCao_DoAn.docx
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

## 7. Backend — ASP.NET Core Web API

### 7.1. Khởi tạo Solution trong Visual Studio 2022

**Bước 1 — Tạo Solution trống:**
```
File → New → Project → "Blank Solution"
Tên Solution: HotelManagement
```

**Bước 2 — Thêm Project API:**
```
Chuột phải Solution → Add → New Project
Chọn: "ASP.NET Core Web API"
Tên: HotelManagement.API
Framework: .NET 8.0
Options:
  ✅ Enable OpenAPI support (Swagger)
  ✅ Use controllers
```

**Bước 3 — Thêm Class Libraries:**
```
Add → New Project → "Class Library (.NET)"
Tên lần lượt:
  - HotelManagement.Models
  - HotelManagement.Repositories
Framework: .NET 8.0
```

**Bước 4 — Thêm Project References:**
```
HotelManagement.API          → References: Models, Repositories
HotelManagement.Repositories → References: Models
```

### 7.2. NuGet Packages cần cài

> **Tools → NuGet Package Manager → Package Manager Console**

```powershell
Install-Package Dapper -ProjectName HotelManagement.Repositories
Install-Package Microsoft.Data.SqlClient -ProjectName HotelManagement.Repositories
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -ProjectName HotelManagement.API
```

### 7.3. Cấu hình appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=HotelManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-minimum-32-chars",
    "Issuer": "HotelManagementAPI",
    "Audience": "HotelManagementClient",
    "ExpiryMinutes": 60
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. CORS — cho phép React gọi API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
        policy.WithOrigins("http://localhost:5173") // Vite default port
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// 3. Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 4. Đăng ký Repositories (Dependency Injection)
builder.Services.AddScoped<IPhongRepository>(sp =>
    new PhongRepository(connectionString!));
builder.Services.AddScoped<IDatPhongRepository>(sp =>
    new DatPhongRepository(connectionString!));
builder.Services.AddScoped<IKhachHangRepository>(sp =>
    new KhachHangRepository(connectionString!));
// ... thêm các repository khác

// 5. JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
        };
    });

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

### 7.5. Ví dụ Repository (Dapper gọi Stored Procedure)

```csharp
using Dapper;
using Microsoft.Data.SqlClient;
using HotelManagement.Models.DTOs;

namespace HotelManagement.Repositories;

public class DatPhongRepository : IDatPhongRepository
{
    private readonly string _connectionString;

    public DatPhongRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> DatPhongAsync(DatPhongDto dto)
    {
        using var conn = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@MaKH", dto.MaKH);
        parameters.Add("@NgayDen", dto.NgayDen);
        parameters.Add("@NgayDi", dto.NgayDi);
        parameters.Add("@MaLoaiPhong", dto.MaLoaiPhong);
        parameters.Add("@SoNguoi", dto.SoNguoi);
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
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DatPhongController : ControllerBase
{
    private readonly IDatPhongRepository _repo;

    public DatPhongController(IDatPhongRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> DatPhong([FromBody] DatPhongDto dto)
    {
        var maDatPhong = await _repo.DatPhongAsync(dto);
        return Ok(new { Success = true, MaDatPhong = maDatPhong });
    }

    [HttpPut("{id}/checkin")]
    public async Task<IActionResult> CheckIn(int id, [FromBody] CheckInDto dto)
    {
        await _repo.CheckInAsync(id, dto.NhanVienXuLy);
        return Ok(new { Success = true });
    }

    [HttpPut("{id}/checkout")]
    public async Task<IActionResult> CheckOut(int id, [FromBody] CheckOutDto dto)
    {
        var hoaDon = await _repo.CheckOutAsync(id, dto.PhuongThucTT);
        return Ok(hoaDon);
    }
}
```

---

## 8. Giao diện (Frontend)

### 8.1. Khởi tạo React + Vite (chạy song song với API)

> React không chạy trong Visual Studio, dùng terminal riêng hoặc VS Code

```powershell
# Trong thư mục gốc dự án
cd frontend
npm create vite@latest . -- --template react
npm install
npm install react-router-dom axios antd recharts

# Chạy dev server
npm run dev
# → http://localhost:5173
```

**Cấu hình Axios trỏ vào ASP.NET Core API:**
```javascript
// src/services/api.js
import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api',
  headers: { 'Content-Type': 'application/json' }
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default api;
```

### 8.2. Các màn hình chính

| Màn hình | Chức năng | Route |
|----------|-----------|-------|
| **Login** | Đăng nhập, xác thực JWT | `/login` |
| **Dashboard** | Thống kê doanh thu, công suất phòng, khách đang ở | `/dashboard` |
| **Quản lý Phòng** | Danh sách phòng, xem sơ đồ phòng trực quan | `/rooms` |
| **Đặt Phòng** | Tìm phòng trống, tạo booking mới | `/bookings/new` |
| **Danh sách Booking** | Xem & duyệt đặt phòng, check-in | `/bookings` |
| **Check-out** | Xem dịch vụ phát sinh, xuất hóa đơn | `/bookings/[id]/checkout` |
| **Khách hàng** | Danh sách, điểm tích lũy, lịch sử lưu trú | `/customers` |
| **Dịch vụ** | Quản lý danh mục dịch vụ | `/services` |
| **Báo cáo** | Doanh thu theo tháng, công suất phòng | `/reports` |
| **Nhân viên** | Quản lý lễ tân & phục vụ | `/staff` |
| **Lịch làm việc** | Phân ca lễ tân / phục vụ (sáng, chiều, đêm) | `/staff/schedule` |

---

## 9. Kế hoạch Thực hiện — 5 Tuần

### 📅 Tuần 1: Thiết kế Database + Khởi tạo Backend
- [ ] Vẽ ERD trên dbdiagram.io & Chuẩn hóa
- [ ] Viết `01_schema.sql` — tạo 10 bảng, constraints
- [ ] Viết `02_indexes.sql`
- [ ] Viết `06_seed_data.sql` — dữ liệu mẫu
- [ ] Khởi tạo Solution trong Visual Studio 2022 (3 projects)
- [ ] Cài NuGet packages: Dapper, SqlClient, JwtBearer
- [ ] Cấu hình `appsettings.json` + `Program.cs`

### 📅 Tuần 2: Database nâng cao + Backend API
- [ ] Viết `03_views.sql` — 6 views
- [ ] Viết `04_stored_procedures.sql` — 6 SPs
- [ ] Viết `05_triggers.sql` — 5 triggers
- [ ] Viết `07_roles_permissions.sql`
- [ ] Viết Models/Entities (10 entities) + DTOs
- [ ] Viết Repositories gọi Stored Procedures qua Dapper
- [ ] Viết Controllers (7 controllers)
- [ ] Test API bằng Swagger UI

### 📅 Tuần 3: Frontend — Layout & Chức năng chính
- [ ] Khởi tạo React + Vite: `npm create vite@latest frontend -- --template react`
- [ ] Cài React Router DOM + Axios + Ant Design + Recharts
- [ ] Cấu hình Axios baseURL trỏ tới ASP.NET Core API
- [ ] Xây dựng layout chung + thanh điều hướng (Sidebar)
- [ ] Trang Login + JWT token flow
- [ ] Trang Dashboard (biểu đồ Recharts)
- [ ] Trang Quản lý Phòng + Sơ đồ phòng trực quan

### 📅 Tuần 4: Frontend — Nghiệp vụ & Báo cáo
- [ ] Trang Đặt phòng + Tìm phòng trống
- [ ] Trang Danh sách Booking + Check-in
- [ ] Trang Check-out + Hóa đơn
- [ ] Trang Quản lý Khách hàng
- [ ] Trang Quản lý Dịch vụ
- [ ] Trang Quản lý Nhân viên + Lịch làm việc
- [ ] Trang Báo cáo doanh thu

### 📅 Tuần 5: Hoàn thiện & Báo cáo
- [ ] Test tích hợp toàn bộ luồng (Book → CheckIn → CheckOut → Invoice)
- [ ] Fix bug, tối ưu UI/UX
- [ ] Viết báo cáo đồ án (Word/Google Docs)
- [ ] Chuẩn bị slide thuyết trình + demo

---

## 10. Tiêu chí Chấm điểm (Web Programming)

| Tiêu chí | Trọng số | Nội dung |
|----------|----------|---------|
| Frontend UI/UX | 30% | Giao diện đẹp, responsive, trải nghiệm người dùng tốt |
| Backend API & Logic | 30% | API chuẩn REST, xử lý logic nghiệp vụ tốt |
| Database Design | 20% | Đúng quan hệ, chuẩn hóa, thiết kế bảng hợp lý |
| Tính năng hệ thống | 10% | Đầy đủ các chức năng quản lý chính |
| Báo cáo & Demo | 10% | Rõ ràng, demo live mượt mà |

---

## 11. Lưu ý Quan trọng khi Dùng Visual Studio 2022

> [!IMPORTANT]
> - Luôn **Build Solution** (`Ctrl+Shift+B`) trước khi chạy để kiểm tra lỗi compile
> - Dùng **Solution Explorer** để quản lý files, không tạo file ngoài VS
> - **Connection String** phải khớp tên SQL Server instance (thường là `localhost\SQLEXPRESS`)
> - Bật **Mixed Mode Authentication** trong SQL Server nếu dùng SQL login

> [!TIP]
> - Dùng **`Ctrl+.`** để auto-fix lỗi và thêm `using` statements
> - Dùng **`F12`** để Go To Definition
> - Dùng **Swagger UI** (tại `/swagger`) để test API nhanh thay vì Postman
> - **Package Manager Console** nhanh hơn UI khi cài nhiều packages

> [!WARNING]
> - Không commit file `appsettings.json` chứa password lên GitHub
> - Thêm `appsettings.json` vào `.gitignore` nếu có thông tin nhạy cảm
> - Dùng `appsettings.Development.json` cho môi trường local

---

## 12. Tài liệu Tham khảo

- 📖 [ASP.NET Core Web API — Docs](https://learn.microsoft.com/en-us/aspnet/core/web-api/) — Tài liệu chính thức
- 📖 [.NET 8 Download](https://dotnet.microsoft.com/download/dotnet/8.0) — Cài .NET SDK
- 📖 [Dapper](https://github.com/DapperLib/Dapper) — Micro ORM gọi Stored Procedures từ C#
- 📖 [Swagger / Swashbuckle](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger) — Tích hợp Swagger vào ASP.NET Core
- 📖 [Visual Studio 2022 Download](https://visualstudio.microsoft.com/vs/) — IDE chính
- 📖 [SQL Server Documentation](https://learn.microsoft.com/en-us/sql/sql-server/)
- 📖 [React + Vite](https://vitejs.dev/guide/) — Khởi tạo project React
- 📖 [Ant Design](https://ant.design/) — Bộ UI component quản lý
- 📖 [Recharts](https://recharts.org/) — Biểu đồ React
