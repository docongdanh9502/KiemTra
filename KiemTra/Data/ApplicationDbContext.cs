using Microsoft.EntityFrameworkCore;
using KiemTra.Models;

namespace KiemTra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HocPhan> HocPhans { get; set; } = null!;
        public DbSet<SinhVien> SinhViens { get; set; } = null!;
        public DbSet<NganhHoc> NganhHocs { get; set; } = null!;
        public DbSet<DangKy> DangKys { get; set; } = null!;
        public DbSet<ChiTietDangKy> ChiTietDangKys { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HocPhan>(entity =>
            {
                entity.HasKey(e => e.MaHP);
                entity.Property(e => e.MaHP).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TenHP).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SoTinChi).IsRequired();
                entity.Property(e => e.SoLuongDuKien).IsRequired();
                entity.Property(e => e.SoLuongDaDangKy).IsRequired();
            });

            modelBuilder.Entity<ChiTietDangKy>(entity =>
            {
                entity.HasKey(e => new { e.MaDK, e.MaHP });
            });
        }
    }
}
