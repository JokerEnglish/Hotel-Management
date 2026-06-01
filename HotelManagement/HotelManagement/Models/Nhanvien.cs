using System;
using System.Collections.Generic;

namespace HotelManagement.Models;

public partial class Nhanvien
{
    public int Manv { get; set; }

    public string Hoten { get; set; } = null!;

    public string Phai { get; set; } = null!;

    public DateOnly? Ngaysinh { get; set; }

    public string Sdt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Diachi { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    public virtual ICollection<Taikhoan> Taikhoans { get; set; } = new List<Taikhoan>();
}
