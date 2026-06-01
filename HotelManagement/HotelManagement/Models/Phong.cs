using System;
using System.Collections.Generic;

namespace HotelManagement.Models;

public partial class Phong
{
    public int Map { get; set; }

    public string Tenphong { get; set; } = null!;

    public int Tinhtrang { get; set; }

    public int Soluongkhachtoida { get; set; }

    public string? Ghichu { get; set; }

    public int Maloaiphong { get; set; }

    public int Songayo { get; set; }

    public virtual ICollection<Khachhang> Khachhangs { get; set; } = new List<Khachhang>();

    public virtual Loaiphong MaloaiphongNavigation { get; set; } = null!;

    public virtual ICollection<Phieuthue> Phieuthues { get; set; } = new List<Phieuthue>();
}
