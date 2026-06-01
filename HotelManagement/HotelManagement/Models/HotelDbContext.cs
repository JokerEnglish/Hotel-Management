using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Models;

public partial class HotelDbContext : DbContext
{
    public HotelDbContext()
    {
    }

    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Loaikhach> Loaikhaches { get; set; }

    public virtual DbSet<Loaiphong> Loaiphongs { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Phieuthue> Phieuthues { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<Phuthu> Phuthus { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.Mahd).HasName("PK__HOADON__603F20CE940C4C9A");

            entity.ToTable("HOADON");

            entity.Property(e => e.Mahd).HasColumnName("MAHD");
            entity.Property(e => e.Cccd)
                .HasMaxLength(50)
                .HasColumnName("CCCD");
            entity.Property(e => e.Manv).HasColumnName("MANV");
            entity.Property(e => e.Ngaydat)
                .HasColumnType("datetime")
                .HasColumnName("NGAYDAT");
            entity.Property(e => e.Ngaylaphd)
                .HasColumnType("datetime")
                .HasColumnName("NGAYLAPHD");
            entity.Property(e => e.Songayo).HasColumnName("SONGAYO");
            entity.Property(e => e.Tenkh)
                .HasMaxLength(50)
                .HasColumnName("TENKH");
            entity.Property(e => e.Tenphong)
                .HasMaxLength(50)
                .HasColumnName("TENPHONG");
            entity.Property(e => e.Tongtien).HasColumnName("TONGTIEN");
            entity.Property(e => e.Tylephuthu).HasColumnName("TYLEPHUTHU");

            entity.HasOne(d => d.ManvNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.Manv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOADON_NHANVIEN");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.Makh).HasName("PK__KHACHHAN__603F592C732FE24D");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.Makh).HasColumnName("MAKH");
            entity.Property(e => e.Cmndkh)
                .HasMaxLength(50)
                .HasColumnName("CMNDKH");
            entity.Property(e => e.Diachikh)
                .HasMaxLength(100)
                .HasColumnName("DIACHIKH");
            entity.Property(e => e.Maloaikhach).HasColumnName("MALOAIKHACH");
            entity.Property(e => e.Map).HasColumnName("MAP");
            entity.Property(e => e.Tel)
                .HasMaxLength(12)
                .HasColumnName("TEL");
            entity.Property(e => e.Tenkh)
                .HasMaxLength(30)
                .HasColumnName("TENKH");
            entity.Property(e => e.Tuoi).HasColumnName("TUOI");

            entity.HasOne(d => d.MaloaikhachNavigation).WithMany(p => p.Khachhangs)
                .HasForeignKey(d => d.Maloaikhach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KHACHHANG_LOAIKHACH");

            entity.HasOne(d => d.MapNavigation).WithMany(p => p.Khachhangs)
                .HasForeignKey(d => d.Map)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KHACHHANG_PHONG1");
        });

        modelBuilder.Entity<Loaikhach>(entity =>
        {
            entity.HasKey(e => e.Maloaikhach).HasName("PK__LOAIKHAC__45AC3DA3FB3D127A");

            entity.ToTable("LOAIKHACH");

            entity.Property(e => e.Maloaikhach).HasColumnName("MALOAIKHACH");
            entity.Property(e => e.Tenloaikhach)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("TENLOAIKHACH");
        });

        modelBuilder.Entity<Loaiphong>(entity =>
        {
            entity.HasKey(e => e.Maloaiphong).HasName("PK__LOAIPHON__82203A767215AE01");

            entity.ToTable("LOAIPHONG");

            entity.Property(e => e.Maloaiphong).HasColumnName("MALOAIPHONG");
            entity.Property(e => e.Dongia).HasColumnName("DONGIA");
            entity.Property(e => e.Tenloai)
                .HasMaxLength(30)
                .HasColumnName("TENLOAI");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.Manv).HasName("PK__NHANVIEN__603F511471B323F6");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.Manv).HasColumnName("MANV");
            entity.Property(e => e.Diachi)
                .HasMaxLength(100)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Hoten)
                .HasMaxLength(30)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Ngaysinh).HasColumnName("NGAYSINH");
            entity.Property(e => e.Phai)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("PHAI");
            entity.Property(e => e.Sdt)
                .HasMaxLength(12)
                .HasColumnName("SDT");
        });

        modelBuilder.Entity<Phieuthue>(entity =>
        {
            entity.HasKey(e => e.Mapt).HasName("PK__PHIEUTHU__603F61D4FB3546B3");

            entity.ToTable("PHIEUTHUE");

            entity.Property(e => e.Mapt).HasColumnName("MAPT");
            entity.Property(e => e.Cccd)
                .HasMaxLength(50)
                .HasColumnName("CCCD");
            entity.Property(e => e.Makh).HasColumnName("MAKH");
            entity.Property(e => e.Map).HasColumnName("MAP");
            entity.Property(e => e.Ngaylappt)
                .HasColumnType("datetime")
                .HasColumnName("NGAYLAPPT");

            entity.HasOne(d => d.MakhNavigation).WithMany(p => p.Phieuthues)
                .HasForeignKey(d => d.Makh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PHIEUTHUE_KHACHHANG1");

            entity.HasOne(d => d.MapNavigation).WithMany(p => p.Phieuthues)
                .HasForeignKey(d => d.Map)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PHIEUTHUE_PHONG");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.Map).HasName("PK__PHONG__C79077809A48578F");

            entity.ToTable("PHONG");

            entity.Property(e => e.Map).HasColumnName("MAP");
            entity.Property(e => e.Ghichu)
                .HasColumnType("text")
                .HasColumnName("GHICHU");
            entity.Property(e => e.Maloaiphong).HasColumnName("MALOAIPHONG");
            entity.Property(e => e.Soluongkhachtoida)
                .HasDefaultValue(2)
                .HasColumnName("SOLUONGKHACHTOIDA");
            entity.Property(e => e.Songayo).HasColumnName("SONGAYO");
            entity.Property(e => e.Tenphong)
                .HasMaxLength(30)
                .HasColumnName("TENPHONG");
            entity.Property(e => e.Tinhtrang)
                .HasDefaultValue(1)
                .HasColumnName("TINHTRANG");

            entity.HasOne(d => d.MaloaiphongNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.Maloaiphong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PHONG_LOAIPHONG");
        });

        modelBuilder.Entity<Phuthu>(entity =>
        {
            entity.HasKey(e => e.Idphuthu).HasName("PK__PHUTHU__EA7046F53F873DD9");

            entity.ToTable("PHUTHU");

            entity.Property(e => e.Idphuthu).HasColumnName("IDPhuthu");
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.Matknv).HasName("PK__TAIKHOAN__67EE5B9AD29A3B3E");

            entity.ToTable("TAIKHOAN");

            entity.Property(e => e.Matknv).HasColumnName("MATKNV");
            entity.Property(e => e.Manv).HasColumnName("MANV");
            entity.Property(e => e.Mktk)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("MKTK");
            entity.Property(e => e.Tentknv)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("TENTKNV");
            entity.Property(e => e.Vaitro)
                .HasMaxLength(20)
                .HasColumnName("VAITRO");

            entity.HasOne(d => d.ManvNavigation).WithMany(p => p.Taikhoans)
                .HasForeignKey(d => d.Manv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TAIKHOAN_NHANVIEN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
