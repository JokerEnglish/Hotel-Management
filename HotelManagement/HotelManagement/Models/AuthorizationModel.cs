namespace HotelManagement.Models
{
    public class AuthorizationModel
    {
        public enum UserRole
        {
            ADMIN,
            NHANVIEN
        }

        public class AuthorizationItem
        {
            public UserRole Role { get; set; }
            public List<String> Permissions { get; set; }
        }
    }
}
