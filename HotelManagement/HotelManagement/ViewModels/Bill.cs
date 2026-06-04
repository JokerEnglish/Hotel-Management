using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.ViewModels
{
    public class Bill
    {
        public Khachhang khachhang { get; set; }
        public List<Phieuthue> phieuthues { get; set; }
        public int PhieuThueId { get; set; }
        public string TenPhong { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal DonGia { get; set; }
    }

}
