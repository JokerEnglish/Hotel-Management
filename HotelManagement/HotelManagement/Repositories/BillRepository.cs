using ClosedXML.Excel;
using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using HotelManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagement.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly HotelDbContext _dbContext;

        public BillRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Hoadon>> GetAllBills()
        {
            return await _dbContext.Hoadons.Include(h => h.ManvNavigation).ToListAsync();
        }

        public async Task<Hoadon> GetBillById(int id)
        {
            return await _dbContext.Hoadons.Include(h => h.ManvNavigation).FirstOrDefaultAsync(h => h.Mahd == id);
        }

        public async Task CreateBill(Hoadon hoadon)
        {
            await _dbContext.Hoadons.AddAsync(hoadon);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBill(Hoadon hoadon)
        {
            _dbContext.Hoadons.Update(hoadon);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBill(int id)
        {
            var hoadon = await _dbContext.Hoadons.FindAsync(id);
            if (hoadon != null)
            {
                _dbContext.Hoadons.Remove(hoadon);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> BillExists(int id)
        {
            return await _dbContext.Hoadons.AnyAsync(h => h.Mahd == id);
        }

        // ===== Báo Cáo Doanh Thu =====

        /// <summary>
        /// Lấy dữ liệu báo cáo doanh thu: JOIN HoaDon + NhanVien + PhieuThue (qua CCCD)
        /// Lọc theo khoảng ngày lập hóa đơn
        /// </summary>
        public async Task<DoanhThuFilterViewModel> GetDoanhThuAsync(DateTime? tuNgay, DateTime? denNgay)
        {
            // Query HOADON kèm NHANVIEN
            var query = _dbContext.Hoadons
                .Include(h => h.ManvNavigation)
                .AsQueryable();

            // Áp dụng bộ lọc ngày
            if (tuNgay.HasValue)
                query = query.Where(h => h.Ngaylaphd.Date >= tuNgay.Value.Date);
            if (denNgay.HasValue)
                query = query.Where(h => h.Ngaylaphd.Date <= denNgay.Value.Date);

            var hoadons = await query.OrderByDescending(h => h.Ngaylaphd).ToListAsync();

            // Lấy tất cả PhieuThue để JOIN thêm thông tin (qua CCCD)
            var phieuthues = await _dbContext.Phieuthues
                .Include(pt => pt.MakhNavigation)
                    .ThenInclude(kh => kh.MaloaikhachNavigation)
                .Include(pt => pt.MapNavigation)
                    .ThenInclude(p => p.MaloaiphongNavigation)
                .ToListAsync();

            // Tạo dictionary CCCD → PhieuThue để tra cứu nhanh
            var ptByCccd = phieuthues
                .GroupBy(pt => pt.Cccd)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());

            // Map sang DoanhThuRowViewModel
            var danhSach = hoadons.Select(hd =>
            {
                ptByCccd.TryGetValue(hd.Cccd, out var pt);
                return new DoanhThuRowViewModel
                {
                    // Hóa đơn
                    Mahd        = hd.Mahd,
                    Tenkh       = hd.Tenkh,
                    Tenphong    = hd.Tenphong,
                    Cccd        = hd.Cccd,
                    Songayo     = hd.Songayo,
                    Ngaydat     = hd.Ngaydat,
                    Ngaylaphd   = hd.Ngaylaphd,
                    Tylephuthu  = hd.Tylephuthu,
                    Tongtien    = hd.Tongtien,

                    // Nhân viên
                    Manv            = hd.Manv,
                    HoTenNhanVien   = hd.ManvNavigation?.Hoten ?? $"NV#{hd.Manv}",

                    // Phiếu thuê (vì đã lưu trong Hóa đơn)
                    Mapt        = hd.Mapt,
                    Ngaylappt   = hd.Ngaydat, // Ngày đặt chính là ngày lập phiếu thuê
                    TenphongPT  = pt?.MapNavigation?.Tenphong ?? hd.Tenphong,
                    TenkhachPT  = pt?.MakhNavigation?.Tenkh ?? hd.Tenkh,
                };
            }).ToList();

            return new DoanhThuFilterViewModel
            {
                TuNgay   = tuNgay,
                DenNgay  = denNgay,
                DanhSach = danhSach,
            };
        }

        /// <summary>
        /// Xuất báo cáo doanh thu ra file Excel (.xlsx) dùng ClosedXML
        /// Sheet 1: Danh sách chi tiết hóa đơn
        /// Sheet 2: Tổng hợp doanh thu
        /// </summary>
        public async Task<byte[]> ExportDoanhThuExcelAsync(DateTime? tuNgay, DateTime? denNgay)
        {
            var vm = await GetDoanhThuAsync(tuNgay, denNgay);
            var data = vm.DanhSach;

            using var workbook = new XLWorkbook();

            // ──────────────────────────────────────────────────────
            // SHEET 1: Chi tiết danh sách hóa đơn
            // ──────────────────────────────────────────────────────
            var ws1 = workbook.Worksheets.Add("Danh Sách Hóa Đơn");

            // --- Tiêu đề chính ---
            ws1.Cell("A1").Value = "BÁO CÁO DOANH THU KHÁCH SẠN";
            ws1.Range("A1:L1").Merge();
            ws1.Cell("A1").Style
                .Font.SetBold(true)
                .Font.SetFontSize(16)
                .Font.SetFontColor(XLColor.White)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#1a237e"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws1.Row(1).Height = 32;

            // --- Khoảng thời gian ---
            string thoiGianStr = "Toàn bộ";
            if (tuNgay.HasValue && denNgay.HasValue)
                thoiGianStr = $"Từ {tuNgay:dd/MM/yyyy} đến {denNgay:dd/MM/yyyy}";
            else if (tuNgay.HasValue)
                thoiGianStr = $"Từ {tuNgay:dd/MM/yyyy}";
            else if (denNgay.HasValue)
                thoiGianStr = $"Đến {denNgay:dd/MM/yyyy}";

            ws1.Cell("A2").Value = $"Kỳ báo cáo: {thoiGianStr}";
            ws1.Range("A2:L2").Merge();
            ws1.Cell("A2").Style
                .Font.SetItalic(true)
                .Font.SetFontSize(11)
                .Font.SetFontColor(XLColor.FromHtml("#424242"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws1.Row(2).Height = 20;

            ws1.Cell("A3").Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws1.Range("A3:L3").Merge();
            ws1.Cell("A3").Style
                .Font.SetItalic(true)
                .Font.SetFontSize(10)
                .Font.SetFontColor(XLColor.FromHtml("#757575"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws1.Row(3).Height = 16;

            // --- Header bảng (dòng 5) ---
            int headerRow = 5;
            var headers = new[]
            {
                "STT", "Mã HĐ", "Tên Khách Hàng", "CCCD", "Tên Phòng",
                "Số Ngày Ở", "Ngày Lập PT (Ngày Đặt)", "Ngày Lập HĐ",
                "Tỷ Lệ Phụ Thu (%)", "Tổng Tiền (VNĐ)",
                "Mã Phiếu Thuê", "Nhân Viên Lập HĐ"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws1.Cell(headerRow, i + 1);
                cell.Value = headers[i];
                cell.Style
                    .Font.SetBold(true)
                    .Font.SetFontSize(11)
                    .Font.SetFontColor(XLColor.White)
                    .Fill.SetBackgroundColor(XLColor.FromHtml("#283593"))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Border.SetOutsideBorderColor(XLColor.FromHtml("#1565c0"));
            }
            ws1.Row(headerRow).Height = 36;

            // --- Dữ liệu ---
            for (int i = 0; i < data.Count; i++)
            {
                int row = headerRow + 1 + i;
                var r = data[i];
                bool isEven = i % 2 == 0;
                var bgColor = isEven ? XLColor.White : XLColor.FromHtml("#e8eaf6");

                void SetCell(int col, object val, XLAlignmentHorizontalValues align = XLAlignmentHorizontalValues.Left)
                {
                    var c = ws1.Cell(row, col);
                    if (val is DateTime dt)
                        c.Value = dt;
                    else if (val is long l)
                        c.Value = l;
                    else if (val is int iv)
                        c.Value = iv;
                    else if (val is double d)
                        c.Value = d;
                    else
                        c.Value = val?.ToString() ?? "";

                    c.Style
                        .Fill.SetBackgroundColor(bgColor)
                        .Alignment.SetHorizontal(align)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Border.SetOutsideBorderColor(XLColor.FromHtml("#c5cae9"));
                }

                SetCell(1, i + 1, XLAlignmentHorizontalValues.Center);
                SetCell(2, r.Mahd, XLAlignmentHorizontalValues.Center);
                SetCell(3, r.Tenkh);
                SetCell(4, r.Cccd, XLAlignmentHorizontalValues.Center);
                SetCell(5, r.Tenphong, XLAlignmentHorizontalValues.Center);
                SetCell(6, r.Songayo, XLAlignmentHorizontalValues.Center);

                // Ngày lập PT (Ngày đặt)
                var cellNgaydat = ws1.Cell(row, 7);
                cellNgaydat.Value = r.Ngaylappt; // Dùng Ngaylappt (tương đương Ngaydat)
                cellNgaydat.Style.NumberFormat.Format = "dd/MM/yyyy";
                cellNgaydat.Style.Fill.SetBackgroundColor(bgColor)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Border.SetOutsideBorderColor(XLColor.FromHtml("#c5cae9"));

                // Ngày lập HĐ
                var cellNgaylaphd = ws1.Cell(row, 8);
                cellNgaylaphd.Value = r.Ngaylaphd;
                cellNgaylaphd.Style.NumberFormat.Format = "dd/MM/yyyy";
                cellNgaylaphd.Style.Fill.SetBackgroundColor(bgColor)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Border.SetOutsideBorderColor(XLColor.FromHtml("#c5cae9"));

                // Tỷ lệ phụ thu
                var cellTyle = ws1.Cell(row, 9);
                cellTyle.Value = r.Tylephuthu;
                cellTyle.Style.NumberFormat.Format = "0.00\"%\"";
                cellTyle.Style.Fill.SetBackgroundColor(bgColor)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Border.SetOutsideBorderColor(XLColor.FromHtml("#c5cae9"));

                // Tổng tiền — format số VNĐ
                var cellTien = ws1.Cell(row, 10);
                cellTien.Value = r.Tongtien;
                cellTien.Style.NumberFormat.Format = "#,##0";
                cellTien.Style.Fill.SetBackgroundColor(bgColor)
                    .Font.SetBold(true)
                    .Font.SetFontColor(XLColor.FromHtml("#1b5e20"))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Border.SetOutsideBorderColor(XLColor.FromHtml("#c5cae9"));

                SetCell(11, r.Mapt.HasValue ? (object)r.Mapt.Value : "Đã xóa", XLAlignmentHorizontalValues.Center);
                SetCell(12, r.HoTenNhanVien);

                ws1.Row(row).Height = 22;
            }

            // --- Dòng tổng cộng ---
            int totalRow = headerRow + 1 + data.Count;
            ws1.Cell(totalRow, 1).Value = "TỔNG CỘNG";
            ws1.Range(totalRow, 1, totalRow, 9).Merge();
            ws1.Cell(totalRow, 1).Style
                .Font.SetBold(true)
                .Font.SetFontSize(12)
                .Font.SetFontColor(XLColor.White)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#1565c0"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            ws1.Cell(totalRow, 10).Value = vm.TongDoanhThu;
            ws1.Cell(totalRow, 10).Style
                .NumberFormat.Format = "#,##0";
            ws1.Cell(totalRow, 10).Style
                .Font.SetBold(true)
                .Font.SetFontSize(12)
                .Font.SetFontColor(XLColor.White)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#1565c0"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            ws1.Range(totalRow, 11, totalRow, 12).Merge();
            ws1.Cell(totalRow, 11).Value = $"{vm.SoHoaDon} hóa đơn";
            ws1.Cell(totalRow, 11).Style
                .Font.SetBold(true)
                .Font.SetFontColor(XLColor.White)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#1565c0"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            ws1.Row(totalRow).Height = 28;

            // --- Auto-fit cột ---
            ws1.Column(1).Width = 5;
            ws1.Column(2).Width = 8;
            ws1.Column(3).Width = 22;
            ws1.Column(4).Width = 16;
            ws1.Column(5).Width = 14;
            ws1.Column(6).Width = 10;
            ws1.Column(7).Width = 15;
            ws1.Column(8).Width = 15;
            ws1.Column(9).Width = 18;
            ws1.Column(10).Width = 18;
            ws1.Column(11).Width = 14;
            ws1.Column(12).Width = 22;

            // Freeze header
            ws1.SheetView.Freeze(headerRow, 0);

            // ──────────────────────────────────────────────────────
            // SHEET 2: Tổng hợp doanh thu
            // ──────────────────────────────────────────────────────
            var ws2 = workbook.Worksheets.Add("Tổng Hợp");

            // Tiêu đề
            ws2.Cell("A1").Value = "TỔNG HỢP DOANH THU";
            ws2.Range("A1:C1").Merge();
            ws2.Cell("A1").Style
                .Font.SetBold(true).Font.SetFontSize(16)
                .Font.SetFontColor(XLColor.White)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#1a237e"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws2.Row(1).Height = 32;

            ws2.Cell("A2").Value = $"Kỳ báo cáo: {thoiGianStr}";
            ws2.Range("A2:C2").Merge();
            ws2.Cell("A2").Style.Font.SetItalic(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws2.Row(2).Height = 18;

            // Bảng tổng hợp
            void AddSummaryRow(int row, string label, object value, string format = "", bool highlight = false)
            {
                ws2.Cell(row, 1).Value = label;
                ws2.Cell(row, 1).Style.Font.SetBold(true).Font.SetFontSize(12)
                    .Fill.SetBackgroundColor(highlight ? XLColor.FromHtml("#e8eaf6") : XLColor.White)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                ws2.Range(row, 1, row, 2).Merge();

                var vc = ws2.Cell(row, 3);
                if (value is long lv) vc.Value = lv;
                else if (value is int iv) vc.Value = iv;
                else if (value is double dv) vc.Value = dv;
                else vc.Value = value?.ToString() ?? "";

                if (!string.IsNullOrEmpty(format))
                    vc.Style.NumberFormat.Format = format;

                vc.Style.Font.SetBold(true).Font.SetFontSize(12)
                    .Fill.SetBackgroundColor(highlight ? XLColor.FromHtml("#283593") : XLColor.FromHtml("#f5f5f5"))
                    .Font.SetFontColor(highlight ? XLColor.White : XLColor.FromHtml("#1b5e20"))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                ws2.Row(row).Height = 28;
            }

            // Header cột
            ws2.Cell(4, 1).Value = "Chỉ Tiêu";
            ws2.Range(4, 1, 4, 2).Merge();
            ws2.Cell(4, 3).Value = "Giá Trị";
            foreach (int col in new[] { 1, 3 })
            {
                ws2.Cell(4, col).Style
                    .Font.SetBold(true).Font.SetFontSize(11).Font.SetFontColor(XLColor.White)
                    .Fill.SetBackgroundColor(XLColor.FromHtml("#283593"))
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            }
            ws2.Row(4).Height = 28;

            AddSummaryRow(5, "Tổng doanh thu (VNĐ)", vm.TongDoanhThu, "#,##0", true);
            AddSummaryRow(6, "Số hóa đơn", vm.SoHoaDon, "", false);
            AddSummaryRow(7, "Doanh thu trung bình / hóa đơn (VNĐ)", (long)vm.TrungBinhMoiHD, "#,##0", false);

            // Thống kê theo tháng (nếu có dữ liệu)
            if (data.Count > 0)
            {
                var byMonth = data
                    .GroupBy(x => new { x.Ngaylaphd.Year, x.Ngaylaphd.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .ToList();

                if (byMonth.Count > 1)
                {
                    ws2.Cell(9, 1).Value = "DOANH THU THEO THÁNG";
                    ws2.Range(9, 1, 9, 3).Merge();
                    ws2.Cell(9, 1).Style
                        .Font.SetBold(true).Font.SetFontSize(13).Font.SetFontColor(XLColor.White)
                        .Fill.SetBackgroundColor(XLColor.FromHtml("#1565c0"))
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws2.Row(9).Height = 28;

                    ws2.Cell(10, 1).Value = "Tháng/Năm";
                    ws2.Cell(10, 2).Value = "Số HĐ";
                    ws2.Cell(10, 3).Value = "Doanh Thu (VNĐ)";
                    for (int c = 1; c <= 3; c++)
                    {
                        ws2.Cell(10, c).Style
                            .Font.SetBold(true).Font.SetFontColor(XLColor.White)
                            .Fill.SetBackgroundColor(XLColor.FromHtml("#283593"))
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    }
                    ws2.Row(10).Height = 24;

                    int mRow = 11;
                    foreach (var g in byMonth)
                    {
                        bool even = (mRow % 2 == 0);
                        var bg = even ? XLColor.White : XLColor.FromHtml("#e8eaf6");
                        ws2.Cell(mRow, 1).Value = $"Tháng {g.Key.Month:D2}/{g.Key.Year}";
                        ws2.Cell(mRow, 2).Value = g.Count();
                        ws2.Cell(mRow, 3).Value = g.Sum(x => x.Tongtien);
                        ws2.Cell(mRow, 3).Style.NumberFormat.Format = "#,##0";
                        for (int c = 1; c <= 3; c++)
                        {
                            ws2.Cell(mRow, c).Style
                                .Fill.SetBackgroundColor(bg)
                                .Alignment.SetHorizontal(c == 1 ? XLAlignmentHorizontalValues.Center : XLAlignmentHorizontalValues.Right)
                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        }
                        ws2.Row(mRow).Height = 22;
                        mRow++;
                    }
                }
            }

            ws2.Column(1).Width = 28;
            ws2.Column(2).Width = 14;
            ws2.Column(3).Width = 22;

            // --- Xuất ra byte[] ---
            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }
    }
}
