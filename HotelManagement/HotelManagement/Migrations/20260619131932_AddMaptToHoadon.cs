using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddMaptToHoadon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOAIKHACH",
                columns: table => new
                {
                    MALOAIKHACH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENLOAIKHACH = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOAIKHAC__45AC3DA3FB3D127A", x => x.MALOAIKHACH);
                });

            migrationBuilder.CreateTable(
                name: "LOAIPHONG",
                columns: table => new
                {
                    MALOAIPHONG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENLOAI = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DONGIA = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LOAIPHON__82203A767215AE01", x => x.MALOAIPHONG);
                });

            migrationBuilder.CreateTable(
                name: "NHANVIEN",
                columns: table => new
                {
                    MANV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HOTEN = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PHAI = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    NGAYSINH = table.Column<DateOnly>(type: "date", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DIACHI = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NHANVIEN__603F511471B323F6", x => x.MANV);
                });

            migrationBuilder.CreateTable(
                name: "PHUTHU",
                columns: table => new
                {
                    IDPhuthu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Giatriphuthu = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PHUTHU__EA7046F53F873DD9", x => x.IDPhuthu);
                });

            migrationBuilder.CreateTable(
                name: "PHONG",
                columns: table => new
                {
                    MAP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENPHONG = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TINHTRANG = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    SOLUONGKHACHTOIDA = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    GHICHU = table.Column<string>(type: "text", nullable: true),
                    MALOAIPHONG = table.Column<int>(type: "int", nullable: false),
                    SONGAYO = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PHONG__C79077809A48578F", x => x.MAP);
                    table.ForeignKey(
                        name: "FK_PHONG_LOAIPHONG",
                        column: x => x.MALOAIPHONG,
                        principalTable: "LOAIPHONG",
                        principalColumn: "MALOAIPHONG");
                });

            migrationBuilder.CreateTable(
                name: "HOADON",
                columns: table => new
                {
                    MAHD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SONGAYO = table.Column<int>(type: "int", nullable: false),
                    MANV = table.Column<int>(type: "int", nullable: false),
                    TONGTIEN = table.Column<int>(type: "int", nullable: false),
                    TENKH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TENPHONG = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NGAYLAPHD = table.Column<DateTime>(type: "datetime", nullable: false),
                    NGAYDAT = table.Column<DateTime>(type: "datetime", nullable: false),
                    TYLEPHUTHU = table.Column<double>(type: "float", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MAPT = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HOADON__603F20CE940C4C9A", x => x.MAHD);
                    table.ForeignKey(
                        name: "FK_HOADON_NHANVIEN",
                        column: x => x.MANV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MANV");
                });

            migrationBuilder.CreateTable(
                name: "TAIKHOAN",
                columns: table => new
                {
                    MATKNV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENTKNV = table.Column<string>(type: "nchar(40)", fixedLength: true, maxLength: 40, nullable: false),
                    MKTK = table.Column<string>(type: "nchar(30)", fixedLength: true, maxLength: 30, nullable: false),
                    MANV = table.Column<int>(type: "int", nullable: false),
                    VAITRO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TAIKHOAN__67EE5B9AD29A3B3E", x => x.MATKNV);
                    table.ForeignKey(
                        name: "FK_TAIKHOAN_NHANVIEN",
                        column: x => x.MANV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MANV");
                });

            migrationBuilder.CreateTable(
                name: "KHACHHANG",
                columns: table => new
                {
                    MAKH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TENKH = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TUOI = table.Column<int>(type: "int", nullable: true),
                    TEL = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    DIACHIKH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CMNDKH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MALOAIKHACH = table.Column<int>(type: "int", nullable: false),
                    MAP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KHACHHAN__603F592C732FE24D", x => x.MAKH);
                    table.ForeignKey(
                        name: "FK_KHACHHANG_LOAIKHACH",
                        column: x => x.MALOAIKHACH,
                        principalTable: "LOAIKHACH",
                        principalColumn: "MALOAIKHACH");
                    table.ForeignKey(
                        name: "FK_KHACHHANG_PHONG1",
                        column: x => x.MAP,
                        principalTable: "PHONG",
                        principalColumn: "MAP");
                });

            migrationBuilder.CreateTable(
                name: "PHIEUTHUE",
                columns: table => new
                {
                    MAPT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NGAYLAPPT = table.Column<DateTime>(type: "datetime", nullable: false),
                    MAKH = table.Column<int>(type: "int", nullable: false),
                    MAP = table.Column<int>(type: "int", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PHIEUTHU__603F61D4FB3546B3", x => x.MAPT);
                    table.ForeignKey(
                        name: "FK_PHIEUTHUE_KHACHHANG1",
                        column: x => x.MAKH,
                        principalTable: "KHACHHANG",
                        principalColumn: "MAKH");
                    table.ForeignKey(
                        name: "FK_PHIEUTHUE_PHONG",
                        column: x => x.MAP,
                        principalTable: "PHONG",
                        principalColumn: "MAP");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HOADON_MANV",
                table: "HOADON",
                column: "MANV");

            migrationBuilder.CreateIndex(
                name: "IX_KHACHHANG_MALOAIKHACH",
                table: "KHACHHANG",
                column: "MALOAIKHACH");

            migrationBuilder.CreateIndex(
                name: "IX_KHACHHANG_MAP",
                table: "KHACHHANG",
                column: "MAP");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUTHUE_MAKH",
                table: "PHIEUTHUE",
                column: "MAKH");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUTHUE_MAP",
                table: "PHIEUTHUE",
                column: "MAP");

            migrationBuilder.CreateIndex(
                name: "IX_PHONG_MALOAIPHONG",
                table: "PHONG",
                column: "MALOAIPHONG");

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOAN_MANV",
                table: "TAIKHOAN",
                column: "MANV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HOADON");

            migrationBuilder.DropTable(
                name: "PHIEUTHUE");

            migrationBuilder.DropTable(
                name: "PHUTHU");

            migrationBuilder.DropTable(
                name: "TAIKHOAN");

            migrationBuilder.DropTable(
                name: "KHACHHANG");

            migrationBuilder.DropTable(
                name: "NHANVIEN");

            migrationBuilder.DropTable(
                name: "LOAIKHACH");

            migrationBuilder.DropTable(
                name: "PHONG");

            migrationBuilder.DropTable(
                name: "LOAIPHONG");
        }
    }
}
