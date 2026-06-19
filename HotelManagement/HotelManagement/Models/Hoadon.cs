using System;
using System.Collections.Generic;

namespace HotelManagement.Models;

public partial class Hoadon
{
    public int Mahd { get; set; }

    public int Songayo { get; set; }

    public int Manv { get; set; }

    public int Tongtien { get; set; }

    public string Tenkh { get; set; } = null!;

    public string Tenphong { get; set; } = null!;

    public DateTime Ngaylaphd { get; set; }

    public DateTime Ngaydat { get; set; }

    public double Tylephuthu { get; set; }

    public string Cccd { get; set; } = null!;

    public int? Mapt { get; set; }

    public virtual Nhanvien ManvNavigation { get; set; } = null!;
}
