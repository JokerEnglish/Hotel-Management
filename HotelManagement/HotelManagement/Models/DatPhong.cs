using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Models;

public partial class DatPhong
{
    [Key]
    public int MaDp { get; set; }

    public int Makh { get; set; }

    public int Map { get; set; }

    public DateTime NgayDat { get; set; }

    public DateTime NgayNhan { get; set; }

    public DateTime NgayTra { get; set; }

    public int TongTienDuKien { get; set; }

    public int Trangthai { get; set; } // 0: Chờ duyệt, 1: Đã duyệt, 2: Đã hủy

    [ForeignKey("Makh")]
    public virtual Khachhang MakhNavigation { get; set; } = null!;

    [ForeignKey("Map")]
    public virtual Phong MapNavigation { get; set; } = null!;
}
