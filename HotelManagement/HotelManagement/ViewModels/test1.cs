using System.ComponentModel.DataAnnotations;

namespace HotelManagement.ViewModels
{
    public class test1
    {
        [DataType(DataType.Date)]
        public DateTime Ngaylaphd { get; set; }

        public string? CCCD { get; set; }

    }
}
