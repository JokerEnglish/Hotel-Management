using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddMaptColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Bổ sung cột Mapt vào bảng Hoadon
            migrationBuilder.AddColumn<int>(
                name: "Mapt",
                table: "Hoadon", // Tùy vào tên bảng thực tế trong DB, có thể là "HOADON" viết hoa
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mapt",
                table: "Hoadon");
        }
    }
}
