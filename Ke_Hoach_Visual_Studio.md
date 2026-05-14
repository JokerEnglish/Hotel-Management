# 🏨 Kế hoạch Thực hiện Dự án — Visual Studio 2022
## Đồ án: Hệ thống Quản lý Chuỗi Khách sạn

---

## 1. Chuẩn bị Môi trường

### 1.1. Cài đặt phần mềm

| Phần mềm | Link | Ghi chú |
|----------|------|---------|
| **Visual Studio 2022** (Community) | https://visualstudio.microsoft.com/ | Chọn Workload: *ASP.NET and web development* |
| **.NET 8 SDK** | https://dotnet.microsoft.com/download/dotnet/8.0 | Tích hợp sẵn trong VS 2022 nếu chọn đúng workload |
| **SQL Server 2022 Express** | https://www.microsoft.com/en-us/sql-server/sql-server-downloads | Bản miễn phí |
| **SSMS** | https://aka.ms/ssmsfullsetup | Quản lý database |
| **Node.js (LTS)** | https://nodejs.org/ | Để chạy React frontend |
| **Git** | https://git-scm.com/ | Quản lý source code |

### 1.2. Workloads cần chọn khi cài Visual Studio 2022

```
✅ ASP.NET and web development
✅ Data storage and processing
✅ Node.js development (nếu muốn chạy React trong VS)
```

### 1.3. Extensions khuyến nghị (Visual Studio 2022)

```
- GitHub Extension for Visual Studio   → Kết nối GitHub
- Bundler & Minifier 2022+             → Tối ưu file tĩnh
- SQL Server Data Tools (SSDT)         → Tích hợp SQL Server trong VS
- REST API Client Tools                → Test API trong VS
```

---

## 2. Khởi tạo Solution trong Visual Studio 2022

### 2.1. Cấu trúc Solution

```
HotelManagement.sln
├── HotelManagement.API         ← ASP.NET Core Web API (.NET 8)
├── HotelManagement.Models      ← Class Library: DTOs, Entities
└── HotelManagement.Repositories ← Class Library: Data Access (Dapper)
```

### 2.2. Các bước tạo Solution

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
  ❌ Configure for HTTPS (bỏ qua khi dev)
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
> Chuột phải Project → Add → Project Reference → chọn project cần tham chiếu

---

## 3. Cài đặt NuGet Packages

> **Tools → NuGet Package Manager → Manage NuGet Packages for Solution**

| Package | Project | Mục đích |
|---------|---------|---------|
| `Dapper` | Repositories | Micro ORM gọi Stored Procedures |
| `Microsoft.Data.SqlClient` | Repositories | Kết nối SQL Server |
| `Swashbuckle.AspNetCore` | API | Swagger UI (thường có sẵn) |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | API | Xác thực JWT |
| `Microsoft.Extensions.Configuration` | Repositories | Đọc appsettings |

**Hoặc dùng Package Manager Console:**
```powershell
# Mở: Tools → NuGet Package Manager → Package Manager Console
Install-Package Dapper -ProjectName HotelManagement.Repositories
Install-Package Microsoft.Data.SqlClient -ProjectName HotelManagement.Repositories
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -ProjectName HotelManagement.API
```

---

## 4. Cấu hình Project

### 4.1. appsettings.json (HotelManagement.API)

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

### 4.2. Program.cs (cấu hình đầy đủ)

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
builder.Services.AddScoped<IKhachSanRepository>(sp =>
    new KhachSanRepository(connectionString!));
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

---

## 5. Cấu trúc Code chi tiết

### 5.1. HotelManagement.Models

```
Models/
├── Entities/
│   ├── KhachSan.cs
│   ├── Phong.cs
│   ├── LoaiPhong.cs
│   ├── KhachHang.cs
│   ├── DatPhong.cs
│   ├── ChiTietDatPhong.cs
│   ├── DichVu.cs
│   ├── SuDungDichVu.cs
│   ├── HoaDon.cs
│   ├── NhanVien.cs
│   ├── GiaPhong.cs
│   └── LichLamViec.cs
└── DTOs/
    ├── DatPhongDto.cs
    ├── CheckInDto.cs
    ├── CheckOutDto.cs
    ├── BaoCaoDto.cs
    └── AuthDto.cs
```

**Ví dụ KhachSan.cs:**
```csharp
namespace HotelManagement.Models.Entities;

public class KhachSan
{
    public int MaKS { get; set; }
    public string TenKS { get; set; } = string.Empty;
    public string DiaChi { get; set; } = string.Empty;
    public string ThanhPho { get; set; } = string.Empty;
    public string? SDT { get; set; }
    public string? Email { get; set; }
    public int HangSao { get; set; }
    public string TrangThai { get; set; } = "HOAT_DONG";
}
```

### 5.2. HotelManagement.Repositories

```
Repositories/
├── Interfaces/
│   ├── IKhachSanRepository.cs
│   ├── IDatPhongRepository.cs
│   ├── IKhachHangRepository.cs
│   └── IBaoCaoRepository.cs
└── Implementations/
    ├── KhachSanRepository.cs
    ├── DatPhongRepository.cs
    ├── KhachHangRepository.cs
    └── BaoCaoRepository.cs
```

**Ví dụ DatPhongRepository.cs:**
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
        parameters.Add("@MaKS", dto.MaKS);
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

### 5.3. HotelManagement.API — Controllers

```
Controllers/
├── AuthController.cs         → POST /api/auth/login
├── KhachSanController.cs     → GET/POST/PUT/DELETE /api/khachsan
├── PhongController.cs        → GET /api/phong/trong
├── DatPhongController.cs     → POST /api/datphong, PUT /api/datphong/checkin
├── KhachHangController.cs    → CRUD /api/khachhang
├── DichVuController.cs       → CRUD /api/dichvu
├── HoaDonController.cs       → GET/POST /api/hoadon
└── BaoCaoController.cs       → GET /api/baocao/doanhtu
```

**Ví dụ DatPhongController.cs:**
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

## 6. Chạy & Debug trong Visual Studio 2022

### 6.1. Chạy API (F5 hoặc Ctrl+F5)

```
- Mở Properties/launchSettings.json để kiểm tra port
- Swagger UI tự mở tại: https://localhost:{PORT}/swagger
- Test API trực tiếp trong Swagger
```

**launchSettings.json:**
```json
{
  "profiles": {
    "HotelManagement.API": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### 6.2. Kết nối SQL Server trong Visual Studio

```
View → SQL Server Object Explorer
→ Add SQL Server → localhost\SQLEXPRESS
→ Chạy SQL scripts trực tiếp trong VS
```

### 6.3. Debug Tips

```
- Đặt Breakpoint: Click lề trái dòng code → F5 để chạy debug
- Watch variables: Debug → Windows → Watch
- Xem SQL query: Dùng SQL Server Profiler (trong SSMS)
- Test API nhanh: Swagger UI tại /swagger
```

---

## 7. Frontend React + Vite (chạy song song)

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

**Cấu hình Axios trỏ vào API:**
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

---

## 8. Kế hoạch Thực hiện theo Tuần

| Tuần | Nội dung | Công cụ chính |
|------|----------|---------------|
| **1–2** | Thiết kế DB, viết schema.sql, seed_data.sql | SSMS + dbdiagram.io |
| **3** | Viết indexes.sql, views.sql | SSMS |
| **4–5** | Stored Procedures + Triggers + Phân quyền | SSMS |
| **6** | Khởi tạo Solution VS2022, cài packages, cấu hình | **Visual Studio 2022** |
| **6** | Viết Models, Repositories (gọi SP qua Dapper) | **Visual Studio 2022** |
| **6** | Viết Controllers, test Swagger | **Visual Studio 2022** |
| **7–8** | Khởi tạo React + Vite, dựng giao diện | VS Code / Terminal |
| **7–8** | Kết nối Frontend → Backend API | VS Code + Postman |
| **9–10** | Fix bug, tối ưu, viết báo cáo, chuẩn bị demo | VS 2022 + VS Code |

---

## 9. Checklist Thực hiện

### ✅ Database (SSMS)
- [ ] Tạo database `HotelManagementDB` trên SQL Server
- [ ] Chạy `01_schema.sql` — tạo 12 bảng
- [ ] Chạy `02_indexes.sql`
- [ ] Chạy `03_views.sql` — 7 views
- [ ] Chạy `04_stored_procedures.sql` — 8 SPs
- [ ] Chạy `05_triggers.sql` — 6 triggers
- [ ] Chạy `06_seed_data.sql` — dữ liệu mẫu
- [ ] Chạy `07_roles_permissions.sql`

### ✅ Backend (Visual Studio 2022)
- [ ] Tạo Solution `HotelManagement.sln`
- [ ] Thêm project `HotelManagement.API` (ASP.NET Core Web API .NET 8)
- [ ] Thêm project `HotelManagement.Models` (Class Library)
- [ ] Thêm project `HotelManagement.Repositories` (Class Library)
- [ ] Cài NuGet: Dapper, SqlClient, JwtBearer
- [ ] Cấu hình `appsettings.json` — ConnectionString
- [ ] Cấu hình `Program.cs` — CORS, JWT, DI
- [ ] Viết Models/Entities (12 entities)
- [ ] Viết Models/DTOs
- [ ] Viết Repositories (gọi Stored Procedures qua Dapper)
- [ ] Viết Controllers (8 controllers)
- [ ] Test toàn bộ API bằng Swagger UI
- [ ] Test JWT Authentication

### ✅ Frontend (VS Code + Terminal)
- [ ] Khởi tạo React + Vite
- [ ] Cài packages: react-router-dom, axios, antd, recharts
- [ ] Cấu hình Axios baseURL
- [ ] Layout chung + Sidebar navigation
- [ ] Trang Dashboard (biểu đồ Recharts)
- [ ] Trang Quản lý Chi nhánh
- [ ] Trang Sơ đồ Phòng (màu sắc theo trạng thái)
- [ ] Trang Đặt Phòng + Danh sách Booking
- [ ] Trang Check-out + Hóa đơn
- [ ] Trang Quản lý Khách hàng
- [ ] Trang Báo cáo Doanh thu
- [ ] Trang Nhân viên + Lịch làm việc
- [ ] Login page + JWT token flow

### ✅ Hoàn thiện
- [ ] Test tích hợp toàn bộ luồng (Book → CheckIn → CheckOut → Invoice)
- [ ] Fix bug
- [ ] Viết báo cáo đồ án
- [ ] Chuẩn bị slide + video demo

---

## 10. Lưu ý Quan trọng khi Dùng Visual Studio 2022

> [!IMPORTANT]
> - Luôn **Build Solution** (`Ctrl+Shift+B`) trước khi chạy để kiểm tra lỗi compile
> - Dùng **Solution Explorer** để quản lý files, không tạo file ngoài VS
> - **Connection String** phải khớp tên SQL Server instance (thường là `localhost\SQLEXPRESS`)
> - Bật **Mixed Mode Authentication** trong SQL Server nếu dùng SQL login

> [!TIP]
> - Dùng **`Ctrl+.`** để auto-fix lỗi và thêm `using` statements
> - Dùng **`F12`** để Go To Definition
> - Dùng **Live Share** để làm nhóm cùng một lúc
> - **Package Manager Console** nhanh hơn UI khi cài nhiều packages

> [!WARNING]
> - Không commit file `appsettings.json` chứa password lên GitHub
> - Thêm `appsettings.json` vào `.gitignore` nếu có thông tin nhạy cảm
> - Dùng `appsettings.Development.json` cho môi trường local

---

## 11. Tài liệu Tham khảo

- 📖 [ASP.NET Core Web API — Docs](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- 📖 [Dapper GitHub](https://github.com/DapperLib/Dapper)
- 📖 [Visual Studio 2022 Download](https://visualstudio.microsoft.com/vs/)
- 📖 [SQL Server Express Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- 📖 [SSMS Download](https://aka.ms/ssmsfullsetup)
- 📖 [Swagger / Swashbuckle](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger)
- 📖 [React + Vite Guide](https://vitejs.dev/guide/)
- 📖 [Ant Design Components](https://ant.design/components/overview/)
