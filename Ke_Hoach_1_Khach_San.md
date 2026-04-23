# 🏨 Đồ án: Hệ thống Quản lý Khách sạn
## Môn: Lập trình Web

---

## 1. Tổng quan Đề tài

Xây dựng hệ thống **Quản lý Khách sạn** hoàn chỉnh gồm:
- **Database** chuyên sâu với SQL Server (stored procedures, triggers, views, transactions, indexing)
- **Backend API** với Node.js + Express để kết nối DB và giao diện
- **Frontend** với React.js (Vite) để tương tác người dùng

> [!IMPORTANT]
> Đây là môn **Lập trình Web**, nên trọng tâm sẽ là sự kết hợp hoàn hảo giữa **Frontend (React)**, **Backend (Node.js)** và **Database (SQL Server)**. Giao diện cần mượt mà, UX/UI tốt và API xử lý logic chặt chẽ.

---

## 2. Công nghệ & Công cụ Sử dụng

### 2.1. Stack công nghệ

```
┌─────────────────────────────────────────┐
│        FRONTEND (React.js + Vite)       │  ← Giao diện người dùng
├─────────────────────────────────────────┤
│          BACKEND (Node.js/Express)      │  ← REST API
├─────────────────────────────────────────┤
│          DATABASE (SQL Server)          │  ← Trọng tâm đồ án
└─────────────────────────────────────────┘
```

### 2.2. Bảng công cụ chi tiết

| Hạng mục | Công cụ | Phiên bản | Ghi chú |
|----------|---------|-----------|---------|
| **DBMS** | SQL Server | 2022 / Express | [microsoft.com/sql-server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) |
| **SQL Client** | SSMS hoặc DBeaver | Latest | [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) |
| **ERD Design** | dbdiagram.io | Online | [dbdiagram.io](https://dbdiagram.io) — Viết DBML, xuất ảnh |
| **Backend Runtime** | Node.js | 20 LTS | [nodejs.org](https://nodejs.org) |
| **Backend Framework** | Express.js | 4.x | REST API |
| **DB Driver** | mssql | Latest | Kết nối SQL Server từ Node |
| **Frontend Framework** | React.js + Vite | 18 + 5.x | Đơn giản, linh hoạt, dễ học |
| **UI Library** | Ant Design hoặc MUI | Latest | Component quản lý sẵn có (table, form, chart) |
| **Routing** | React Router DOM | v6 | Điều hướng giữa các trang |
| **HTTP Client** | Axios | Latest | Gọi REST API từ frontend |
| **Biểu đồ** | Recharts | Latest | Dashboard doanh thu, công suất |
| **Code Editor** | VS Code | Latest | Extension: SQL Server, ESLint |
| **API Test** | Thunder Client / Postman | Latest | Test API trước khi kết nối UI |
| **Version Control** | Git + GitHub | — | Quản lý source code |
| **Báo cáo** | Google Docs | — | Viết tài liệu đồ án |

### 2.3. VS Code Extensions cần cài

```
- SQL Server (mssql) (Microsoft)  → Chạy SQL ngay trong VS Code
- ESLint + Prettier           → Format code
- Thunder Client              → Test REST API
- GitLens                     → Quản lý Git
```

---

## 3. Cấu trúc Thư mục Dự án

```
hotel-management/
├── database/                   ← TẤT CẢ SCRIPT SQL
│   ├── 01_schema.sql           → Tạo bảng, constraints
│   ├── 02_indexes.sql          → Tạo index
│   ├── 03_views.sql            → Tạo views
│   ├── 04_stored_procedures.sql→ Stored procedures
│   ├── 05_triggers.sql         → Triggers
│   ├── 06_seed_data.sql        → Dữ liệu mẫu
│   └── 07_roles_permissions.sql→ Phân quyền
│
├── backend/                    ← NODE.JS API
│   ├── src/
│   │   ├── config/db.js        → Kết nối SQL Server
│   │   ├── routes/
│   │   │   ├── rooms.js        → API phòng
│   │   │   ├── bookings.js     → API đặt phòng
│   │   │   ├── customers.js    → API khách hàng
│   │   │   ├── services.js     → API dịch vụ
│   │   │   └── reports.js      → API báo cáo
│   │   └── index.js
│   └── package.json
│
├── frontend/                   ← REACT.JS (VITE) UI
│   ├── src/
│   │   ├── pages/
│   │   │   ├── Dashboard.jsx   → Trang tổng quan
│   │   │   ├── Rooms.jsx       → Quản lý phòng
│   │   │   ├── Bookings.jsx    → Quản lý đặt phòng
│   │   │   ├── Customers.jsx   → Quản lý khách hàng
│   │   │   ├── Staff.jsx       → Quản lý nhân viên
│   │   │   └── Reports.jsx     → Báo cáo doanh thu
│   │   ├── components/         → Các component dùng chung
│   │   ├── services/           → Gọi API (axios)
│   │   └── App.jsx
│   └── package.json
│
└── docs/                       ← TÀI LIỆU
    ├── ERD.png
    └── BaoCao_DoAn.docx
```

---

## 4. Phân tích Nghiệp vụ

### 4.1. Các Actor
| Actor | Vai trò |
|-------|----------|
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

## 7. Giao diện (Frontend)

### 7.1. Các màn hình chính

| Màn hình | Chức năng | Route |
|----------|-----------|-------|
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

## 8. Kế hoạch Thực hiện theo Tuần

### 📅 Tuần 1–2: Thiết kế Database
- [ ] Vẽ ERD & Chuẩn hóa
- [ ] Viết script tạo bảng & constraints (SQL Server)
- [ ] Tạo dữ liệu mẫu (Seed data)

### 📅 Tuần 3–4: Backend API
- [ ] Khởi tạo project Node.js + Express
- [ ] Kết nối SQL Server qua `mssql`
- [ ] Viết REST API gọi stored procedures
- [ ] Test API bằng Thunder Client

### 📅 Tuần 5–7: Frontend (React.js + Vite)
- [ ] Khởi tạo project: `npm create vite@latest frontend -- --template react`
- [ ] Cài React Router DOM + Axios + Ant Design
- [ ] Xây dựng Dashboard & Sơ đồ Phòng
- [ ] Màn hình Đặt phòng & Check-in/out
- [ ] Màn hình Báo cáo doanh thu

### 📅 Tuần 8: Hoàn thiện & Báo cáo
- [ ] Fix bug, tối ưu UI
- [ ] Viết báo cáo đồ án
- [ ] Chuẩn bị slide thuyết trình + demo

---

## 9. Tiêu chí Chấm điểm (Web Programming)

| Tiêu chí | Trọng số | Nội dung |
|----------|----------|---------|
| Frontend UI/UX | 30% | Giao diện đẹp, responsive, trải nghiệm người dùng tốt |
| Backend API & Logic | 30% | API chuẩn REST, xử lý logic nghiệp vụ tốt |
| Database Design | 20% | Đúng quan hệ, chuẩn hóa, thiết kế bảng hợp lý |
| Tính năng hệ thống | 10% | Đầy đủ các chức năng quản lý chính |
| Báo cáo & Demo | 10% | Rõ ràng, demo live mượt mà |

---

## 10. Tài liệu Tham khảo

- 📖 [SQL Server Documentation](https://learn.microsoft.com/en-us/sql/sql-server/)
- 📖 [node-mssql](https://github.com/tediousjs/node-mssql) — Kết nối SQL Server từ Node
- 📖 [React + Vite](https://vitejs.dev/guide/) — Khởi tạo project React
- 📖 [Ant Design](https://ant.design/) — Bộ UI component quản lý
- 📖 [Recharts](https://recharts.org/) — Biểu đồ React
