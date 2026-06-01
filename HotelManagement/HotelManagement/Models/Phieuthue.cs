using System;
using System.Collections.Generic;

namespace HotelManagement.Models;

public partial class Phieuthue
{
    public int Mapt { get; set; }

    public DateTime Ngaylappt { get; set; }

    public int Makh { get; set; }

    public int Map { get; set; }

    public string Cccd { get; set; } = null!;

    public virtual Khachhang MakhNavigation { get; set; } = null!;

    public virtual Phong MapNavigation { get; set; } = null!;
}
