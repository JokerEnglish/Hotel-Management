namespace HotelManagement.ViewModels
{
    /// <summary>
    /// Mỗi dòng trong báo cáo doanh thu: kết hợp thông tin Hóa Đơn + Nhân Viên
    /// </summary>
    public class DoanhThuRowViewModel
    {
        // ===== Thông tin Hóa Đơn =====
        public int Mahd { get; set; }
        public string Tenkh { get; set; } = string.Empty;
        public string Tenphong { get; set; } = string.Empty;
        public string Cccd { get; set; } = string.Empty;
        public int Songayo { get; set; }
        public DateTime Ngaydat { get; set; }
        public DateTime Ngaylaphd { get; set; }
        public double Tylephuthu { get; set; }
        public long Tongtien { get; set; }

        // ===== Thông tin Nhân Viên lập HĐ =====
        public int Manv { get; set; }
        public string HoTenNhanVien { get; set; } = string.Empty;

        // ===== Thông tin Phiếu Thuê liên quan (join qua CCCD) =====
        public int? Mapt { get; set; }
        public DateTime? Ngaylappt { get; set; }
        public string TenphongPT { get; set; } = string.Empty;   // Tên phòng từ bảng Phong (qua Phieuthue)
        public string TenkhachPT { get; set; } = string.Empty;   // Tên khách từ bảng Khachhang (qua Phieuthue)
    }

    /// <summary>
    /// ViewModel toàn trang báo cáo doanh thu: bao gồm bộ lọc + danh sách + tổng hợp
    /// </summary>
    public class DoanhThuFilterViewModel
    {
        // Bộ lọc thời gian
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }

        // Danh sách kết quả
        public List<DoanhThuRowViewModel> DanhSach { get; set; } = new();

        // Thống kê tổng hợp
        public long TongDoanhThu => DanhSach.Sum(x => x.Tongtien);
        public int SoHoaDon => DanhSach.Count;
        public double TrungBinhMoiHD => SoHoaDon > 0 ? (double)TongDoanhThu / SoHoaDon : 0;
    }
}
