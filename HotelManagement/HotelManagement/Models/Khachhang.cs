using System;
using System.Collections.Generic;

namespace HotelManagement.Models;

public partial class Khachhang
{
    public int Makh { get; set; }

    public string Tenkh { get; set; } = null!;

    public int? Tuoi { get; set; }

    public string? Tel { get; set; }

    public string? Diachikh { get; set; }

    public string Cmndkh { get; set; } = null!;

    public int Maloaikhach { get; set; }

    public int Map { get; set; }

    public virtual Loaikhach MaloaikhachNavigation { get; set; } = null!;

    public virtual Phong MapNavigation { get; set; } = null!;

    public virtual ICollection<Phieuthue> Phieuthues { get; set; } = new List<Phieuthue>();
}
