using System;
using System.Collections.Generic;

namespace HotelManagement.Models;

public partial class Taikhoan
{
    public int Matknv { get; set; }

    public string Tentknv { get; set; } = null!;

    public string Mktk { get; set; } = null!;

    public int Manv { get; set; }

    public string Vaitro { get; set; } = null!;

    public virtual Nhanvien ManvNavigation { get; set; } = null!;
}
