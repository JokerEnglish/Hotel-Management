using System;
using System.Collections.Generic;

namespace HotelManagement.Models;

public partial class Loaiphong
{
    public int Maloaiphong { get; set; }

    public string Tenloai { get; set; } = null!;

    public int Dongia { get; set; }

    public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
}
