using System.ComponentModel.DataAnnotations;

namespace HotelManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [StringLength(30, ErrorMessage = "Tên không được vượt quá 30 ký tự")]
        public string Tenkh { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập CMND/CCCD")]
        [StringLength(50, ErrorMessage = "CMND không được vượt quá 50 ký tự")]
        public string Cmndkh { get; set; } = null!;

        public int? Tuoi { get; set; }

        [StringLength(12, ErrorMessage = "SĐT không được vượt quá 12 ký tự")]
        public string? Tel { get; set; }

        [StringLength(100, ErrorMessage = "Địa chỉ không được vượt quá 100 ký tự")]
        public string? Diachikh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Loại khách")]
        public int Maloaikhach { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } = null!;
    }
}
