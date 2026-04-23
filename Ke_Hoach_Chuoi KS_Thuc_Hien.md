# 🏨 Đồ án: Hệ thống Quản lý Chuỗi Khách sạn
## Môn: Lập trình Web

---

## 1. Tổng quan Đề tài

Xây dựng hệ thống **Quản lý Chuỗi Khách sạn** hoàn chỉnh — quản lý nhiều chi nhánh khách sạn trong cùng một hệ thống:
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
| **Code Editor** | VS Code | Latest | Extension: PostgreSQL, ESLint |
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
│   │   │   ├── Hotels.jsx      → Quản lý chi nhánh
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
| Actor | Phạm vi | Vai trò |
|-------|---------|----------|
| **Admin (Tổng công ty)** | Toàn chuỗi | Quản lý tất cả chi nhánh, xem báo cáo tổng hợp |
| **Quản lý chi nhánh** | 1 khách sạn | Quản lý nhân viên, xem báo cáo chi nhánh |
| **Nhân viên Lễ tân** | 1 khách sạn | Đặt phòng, check-in, check-out, lập hóa đơn |
| **Nhân viên Phục vụ** | 1 khách sạn | Ghi nhận dịch vụ phát sinh (ăn uống, spa...) |
| **Khách hàng** | Toàn chuỗi | Đặt phòng bất kỳ chi nhánh, tích điểm chung |

### 4.2. Chức năng chính
- ✅ Quản lý danh sách khách sạn (chuỗi chi nhánh)
- ✅ Quản lý phòng & loại phòng theo từng chi nhánh
- ✅ Quản lý giá phòng linh hoạt theo chi nhánh & thời điểm
- ✅ Đặt phòng (Booking) — trực tiếp & online, bất kỳ chi nhánh
- ✅ Check-in / Check-out
- ✅ Quản lý dịch vụ bổ sung (ăn uống, spa, giặt ủi...)
- ✅ Quản lý khách hàng & điểm tích lũy **dùng chung toàn chuỗi**
- ✅ Quản lý hóa đơn & thanh toán
- ✅ Báo cáo doanh thu: theo chi nhánh & **tổng hợp toàn chuỗi**

---

## 5. Thiết kế Cơ sở Dữ liệu

### 5.1. Các bảng chính (12 bảng)

```sql
-- 1. ⭐ Khách sạn (Bảng gốc của chuỗi)
KhachSan (MaKS, TenKS, DiaChi, ThanhPho, QuocGia, SDT, Email, HangSao,
          SoPhongToiDa, NgayKhaiTruong, TrangThai)
-- TrangThai: 'HOAT_DONG', 'TAM_DONG', 'DANG_XAY'

-- 2. Loại phòng (dùng chung toàn chuỗi)
LoaiPhong (MaLoaiPhong, TenLoai, MoTa, SucChuaToiDa, TienNghi, HinhAnh)

-- 3. ⭐ Giá phòng (linh hoạt theo chi nhánh & thời điểm)
GiaPhong (MaGia, MaKS, MaLoaiPhong, GiaMoiDem, NgayApDung, NgayHetHan, GhiChu)
-- Mỗi chi nhánh có thể có giá khác nhau

-- 4. Phòng (thuộc về 1 khách sạn cụ thể)
Phong (MaPhong, MaKS, SoPhong, Tang, MaLoaiPhong, TrangThai, MoTa, HinhAnh)
-- TrangThai: 'TRONG', 'DA_DAT', 'DANG_O', 'BAO_TRI'

-- 5. Khách hàng (tích điểm CHUNG toàn chuỗi)
KhachHang (MaKH, HoTen, CCCD, DiaChi, SDT, Email, NgaySinh, LoaiKH, DiemTichLuy)
-- LoaiKH: 'THUONG', 'THAN_THIET', 'VIP'

-- 6. Đặt phòng (có thể đặt bất kỳ chi nhánh)
DatPhong (MaDatPhong, MaKH, MaKS, NgayDat, NgayDen, NgayDi, SoNguoiLon, SoTreEm,
          TrangThai, TongTien, GhiChu, KenhDat, NhanVienXuLy)
-- TrangThai: 'CHO_XAC_NHAN', 'DA_XAC_NHAN', 'DA_CHECKIN', 'DA_CHECKOUT', 'HUY'

-- 7. Chi tiết đặt phòng
ChiTietDatPhong (MaChiTiet, MaDatPhong, MaPhong, GiaThucTe, GhiChu)

-- 8. Dịch vụ (có thể theo từng chi nhánh hoặc toàn chuỗi)
DichVu (MaDichVu, MaKS, TenDichVu, MoTa, DonGia, DonViTinh, LoaiDichVu, TrangThai)
-- MaKS = NULL nghĩa là dịch vụ áp dụng toàn chuỗi

-- 9. Sử dụng dịch vụ
SuDungDichVu (MaSuDung, MaDatPhong, MaDichVu, SoLuong, DonGia, ThoiGian, GhiChu)

-- 10. Hóa đơn
HoaDon (MaHoaDon, MaDatPhong, NgayLap, TienPhong, TienDichVu,
        GiamGia, ThueVAT, TongCong, PhuongThucTT, TrangThaiTT, NhanVienThu)

-- 11. Nhân viên (thuộc 1 chi nhánh cụ thể)
NhanVien (MaNV, MaKS, HoTen, CCCD, LoaiNV, PhongBan, SDT, Email, LuongCoBan, NgayVaoLam, TrangThai)
-- LoaiNV: 'LE_TAN' | 'PHUC_VU' | 'QUAN_LY' | 'KE_TOAN'
-- Lễ tân: nhận đặt phòng, check-in, check-out, lập hóa đơn
-- Phục vụ: ghi nhận dịch vụ phát sinh cho khách

-- 12. Lịch làm việc
LichLamViec (MaLich, MaNV, NgayLam, CaLam, GioVao, GioRa, GhiChu)
-- CaLam: 'SANG' (6h-14h) | 'CHIEU' (14h-22h) | 'DEM' (22h-6h)
```

### 5.2. Sơ đồ ERD (tổng quan)
```
KhachSan ──< GiaPhong >── LoaiPhong
    │
    ├──< Phong >── LoaiPhong
    │
    ├──< DatPhong ──< KhachHang
    │       │
    │       ├──< ChiTietDatPhong >── Phong
    │       ├──< SuDungDichVu >── DichVu
    │       └──< HoaDon
    │
    └──< NhanVien ──< LichLamViec
```

---

## 6. Kỹ thuật CSDL Nâng cao (Trọng tâm chấm điểm)

### 6.1. Stored Procedures
```sql
sp_DatPhong(MaKH, MaKS, NgayDen, NgayDi, MaLoaiPhong, SoNguoi)
sp_CheckIn(MaDatPhong, NhanVienXuLy)
sp_CheckOut(MaDatPhong, PhuongThucThanhToan)
sp_TimPhongTrong(MaKS, NgayDen, NgayDi, MaLoaiPhong, SoNguoi)  -- tìm theo chi nhánh
sp_BaoCaoDoanhThuChiNhanh(MaKS, Thang, Nam)                    -- báo cáo 1 chi nhánh
sp_BaoCaoDoanhThuToanChuan(Thang, Nam)                          -- báo cáo tổng chuỗi
sp_CapNhatDiemTichLuy(MaKH, SoTien)                            -- điểm dùng chung
sp_GetGiaPhong(MaKS, MaLoaiPhong, NgayDen)                     -- lấy giá đúng thời điểm
```

### 6.2. Triggers
```sql
trg_CapNhatTrangThaiPhong     -- Tự động cập nhật phòng khi check-in/out
trg_TinhTongTienHoaDon        -- Tự động tính lại tổng tiền khi thêm DV
trg_LogThayDoiGiaPhong        -- Ghi log khi thay đổi giá (theo từng chi nhánh)
trg_KiemTraDatPhongTrung      -- Cảnh báo đặt phòng trùng trong cùng chi nhánh
trg_CapNhatDiemSauCheckout    -- Cộng điểm tích lũy CHUNG toàn chuỗi sau checkout
trg_KiemTraGiaHopLe           -- Cảnh báo nếu thêm giá mới bị trùng khoảng thời gian
```

### 6.3. Views
```sql
v_PhongTrongTheoKS            -- Phòng trống theo từng chi nhánh
v_KhachDangO                  -- Danh sách khách đang lưu trú (toàn chuỗi)
v_DoanhThuTheoChiNhanh        -- Doanh thu so sánh giữa các chi nhánh
v_DoanhThuTheoLoaiPhong       -- Doanh thu theo loại phòng
v_LichSuLuuTru                -- Lịch sử lưu trú của khách hàng (toàn chuỗi)
v_CongSuatPhongTheoKS         -- Công suất phòng theo tháng, theo chi nhánh
v_KhachHangVIP                -- Khách hàng VIP với điểm tích lũy cao nhất
```

### 6.4. Indexing & Transactions & Security
```sql
-- Index
CREATE INDEX idx_datphong_ngay ON DatPhong(NgayDen, NgayDi, TrangThai);
CREATE INDEX idx_khachhang_sdt ON KhachHang(SDT);

-- Transaction (check-out)
BEGIN → Tạo hóa đơn → Cập nhật trạng thái → Cộng điểm → COMMIT/ROLLBACK

-- Phân quyền theo loại nhân viên
ROLE: Admin | QuanLyChiNhanh | LeTan | NhanVienPhucVu | KeToan
-- LeTan     : SELECT/INSERT/UPDATE trên DatPhong, HoaDon, KhachHang
-- PhucVu    : INSERT/UPDATE trên SuDungDichVu, xem DatPhong
-- QuanLy    : Tất cả trong chi nhánh + xem báo cáo
-- Admin     : Toàn quyền toàn chuỗi
```

---

## 7. Giao diện (Frontend)

### 7.1. Các màn hình chính

| Màn hình | Chức năng | Route |
|----------|-----------|-------|
| **Dashboard Tổng** | Thống kê toàn chuỗi: doanh thu, công suất, khách đang ở | `/dashboard` |
| **Quản lý Chi nhánh** | Danh sách khách sạn, thêm/sửa chi nhánh | `/hotels` |
| **Sơ đồ Phòng** | Chọn chi nhánh → xem sơ đồ phòng trực quan | `/hotels/[id]/rooms` |
| **Quản lý Giá** | Cài giá phòng theo chi nhánh & thời điểm | `/hotels/[id]/pricing` |
| **Đặt Phòng** | Tìm phòng trống theo chi nhánh & ngày | `/bookings/new` |
| **Danh sách Booking** | Xem & duyệt đặt phòng, check-in | `/bookings` |
| **Check-out** | Xem dịch vụ phát sinh, xuất hóa đơn | `/bookings/[id]/checkout` |
| **Khách hàng** | Danh sách, điểm tích lũy, lịch sử toàn chuỗi | `/customers` |
| **Dịch vụ** | Quản lý dịch vụ theo chi nhánh hoặc toàn chuỗi | `/services` |
| **Báo cáo** | Doanh thu so sánh chi nhánh, tổng hợp chuỗi | `/reports` |
| **Nhân viên** | Quản lý lễ tân & phục vụ theo từng chi nhánh | `/staff` |
| **Lịch làm việc** | Phân ca lễ tân / phục vụ (sáng, chiều, đêm) | `/staff/schedule` |

### 7.2. UI Highlights
- 🗺️ **Sơ đồ phòng trực quan** — hiển thị trạng thái từng phòng theo màu sắc
- 📊 **Dashboard với biểu đồ** — doanh thu, công suất (dùng Recharts)
- 🔍 **Tìm phòng trống** — chọn ngày → hiển thị phòng available real-time
- 🧾 **Xuất hóa đơn PDF** — khi checkout

---

## 8. Kế hoạch Thực hiện theo Tuần

### 📅 Tuần 1–2: Thiết kế Database
- [ ] Vẽ ERD trên dbdiagram.io
- [ ] Chuẩn hóa đến 3NF/BCNF
- [ ] Viết `01_schema.sql` — tạo bảng, constraints
- [ ] Viết `06_seed_data.sql` — dữ liệu mẫu (50+ records/bảng)

### 📅 Tuần 3: Index + Views
- [ ] Viết `02_indexes.sql`
- [ ] Viết `03_views.sql` — 5 views chính

### 📅 Tuần 4–5: Stored Procedures + Triggers
- [ ] Viết `04_stored_procedures.sql` — 6 SP chính
- [ ] Viết `05_triggers.sql` — 5 triggers
- [ ] Viết `07_roles_permissions.sql`
- [ ] Test toàn bộ bằng DBeaver

### 📅 Tuần 6: Backend API
- [ ] Khởi tạo project Node.js + Express
- [ ] Kết nối SQL Server qua `mssql`
- [ ] Viết REST API gọi stored procedures
- [ ] Test API bằng Thunder Client

### 📅 Tuần 7–8: Frontend (React.js + Vite)
- [ ] Khởi tạo project: `npm create vite@latest frontend -- --template react`
- [ ] Cài React Router DOM + Axios + Ant Design (hoặc MUI)
- [ ] Xây dựng layout chung + thanh điều hướng
- [ ] Màn hình Dashboard (biểu đồ Recharts)
- [ ] Màn hình Quản lý Chi nhánh & Sơ đồ Phòng
- [ ] Màn hình Đặt phòng + Danh sách Booking
- [ ] Màn hình Check-out + Hóa đơn
- [ ] Màn hình Quản lý Nhân viên (Lễ tân / Phục vụ) + Lịch làm việc
- [ ] Màn hình Báo cáo doanh thu theo chi nhánh

### 📅 Tuần 9–10: Hoàn thiện & Báo cáo
- [ ] Fix bug, tối ưu UI
- [ ] Viết báo cáo đồ án (Word/Google Docs)
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

## 10. Gợi ý Tính năng Nổi bật (Điểm cộng)

> [!TIP]
> Thêm các tính năng sau để gây ấn tượng với giảng viên:
> 1. **Table Partitioning** — Phân vùng bảng `DatPhong` theo năm
> 2. **Materialized View** — Cache báo cáo doanh thu tháng, refresh định kỳ
> 3. **Audit Log** — Bảng ghi lại mọi thay đổi dữ liệu quan trọng
> 4. **Full-text Search** — Tìm khách hàng theo tên bằng GIN index
> 5. **Xuất PDF hóa đơn** — Dùng thư viện `pdfkit` hoặc `react-pdf`
> 6. **Biểu đồ dashboard** — Recharts (doanh thu, công suất phòng)

---

## 11. Tài liệu Tham khảo

- 📖 [SQL Server Documentation](https://learn.microsoft.com/en-us/sql/sql-server/)
- 📖 [dbdiagram.io](https://dbdiagram.io) — Vẽ ERD online
- 📖 [node-mssql](https://github.com/tediousjs/node-mssql) — Kết nối SQL Server từ Node
- 📖 [React + Vite](https://vitejs.dev/guide/) — Khởi tạo project React
- 📖 [React Router DOM](https://reactrouter.com/) — Routing cho React
- 📖 [Ant Design](https://ant.design/) — Bộ UI component quản lý
- 📖 [Axios](https://axios-http.com/) — HTTP client
- 📖 [Use The Index Luke](https://use-the-index-luke.com/) — Tối ưu index
- 📖 [Recharts](https://recharts.org/) — Biểu đồ React
