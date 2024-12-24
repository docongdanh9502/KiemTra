using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KiemTra.Models;
using KiemTra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KiemTra.Controllers
{
    public class DangKyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DangKyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hocPhans = await _context.HocPhans.ToListAsync();
            return View(hocPhans);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKyHocPhan(string maHP)
        {
            var hocPhan = await _context.HocPhans.FindAsync(maHP);
            if (hocPhan == null)
            {
                return NotFound();
            }

            var maSV = User.Identity?.Name;
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .FirstOrDefaultAsync(d => d.MaSV == maSV);

            if (dangKy == null)
            {
                dangKy = new DangKy
                {
                    MaSV = maSV,
                    NgayDK = DateTime.Now,
                    ChiTietDangKys = new List<ChiTietDangKy>()
                };
                _context.DangKys.Add(dangKy);
                await _context.SaveChangesAsync();
            }

            var existingChiTiet = await _context.ChiTietDangKys
                .FirstOrDefaultAsync(c => c.MaDK == dangKy.MaDK && c.MaHP == maHP);

            if (existingChiTiet != null)
            {
                TempData["ErrorMessage"] = "Học phần này đã được đăng ký!";
                return RedirectToAction(nameof(DanhSachDaDangKy));
            }

            if (hocPhan.SoLuongDaDangKy >= hocPhan.SoLuongDuKien)
            {
                TempData["ErrorMessage"] = "Học phần đã đủ số lượng đăng ký!";
                return RedirectToAction(nameof(DanhSachDaDangKy));
            }

            var chiTietDangKy = new ChiTietDangKy
            {
                MaDK = dangKy.MaDK,
                MaHP = maHP
            };

            _context.ChiTietDangKys.Add(chiTietDangKy);
            hocPhan.SoLuongDaDangKy++;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(DanhSachDaDangKy));
        }

        public async Task<IActionResult> DanhSachDaDangKy()
        {
            var maSV = User.Identity?.Name;
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(d => d.MaSV == maSV);

            if (dangKy == null)
            {
                dangKy = new DangKy
                {
                    MaSV = maSV,
                    NgayDK = DateTime.Now,
                    ChiTietDangKys = new List<ChiTietDangKy>()
                };
            }

            return View(dangKy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaHocPhan(int maDK, string maHP)
        {
            var chiTietDangKy = await _context.ChiTietDangKys
                .Include(c => c.HocPhan)
                .FirstOrDefaultAsync(c => c.MaDK == maDK && c.MaHP == maHP);

            if (chiTietDangKy != null)
            {
                var hocPhan = chiTietDangKy.HocPhan;
                if (hocPhan != null)
                {
                    hocPhan.SoLuongDaDangKy--;
                }
                _context.ChiTietDangKys.Remove(chiTietDangKy);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(DanhSachDaDangKy));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaTatCa()
        {
            var maSV = User.Identity?.Name;
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .FirstOrDefaultAsync(d => d.MaSV == maSV);

            if (dangKy != null && dangKy.ChiTietDangKys.Any())
            {
                foreach (var chiTiet in dangKy.ChiTietDangKys)
                {
                    if (chiTiet.HocPhan != null)
                    {
                        chiTiet.HocPhan.SoLuongDaDangKy--;
                    }
                }
                _context.ChiTietDangKys.RemoveRange(dangKy.ChiTietDangKys);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(DanhSachDaDangKy));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LuuDangKy(int maDK)
        {
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .Include(d => d.SinhVien)
                .ThenInclude(s => s.NganhHoc)
                .FirstOrDefaultAsync(d => d.MaDK == maDK);

            if (dangKy != null)
            {
                dangKy.NgayDK = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThongTinDangKy), new { id = maDK });
            }

            return RedirectToAction(nameof(DanhSachDaDangKy));
        }

        public async Task<IActionResult> ThongTinDangKy(int id)
        {
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .Include(d => d.SinhVien)
                .ThenInclude(s => s.NganhHoc)
                .FirstOrDefaultAsync(d => d.MaDK == id);

            if (dangKy == null)
            {
                return NotFound();
            }

            return View(dangKy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhanDangKy(int id)
        {
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .FirstOrDefaultAsync(d => d.MaDK == id);

            if (dangKy != null)
            {
                dangKy.NgayDK = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThongTinDaLuu), new { id = dangKy.MaDK });
            }

            return RedirectToAction(nameof(ThongTinDangKy), new { id });
        }

        public async Task<IActionResult> ThongTinDaLuu(int id)
        {
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .Include(d => d.SinhVien)
                .ThenInclude(s => s.NganhHoc)
                .FirstOrDefaultAsync(d => d.MaDK == id);

            if (dangKy == null)
            {
                return NotFound();
            }

            return View(dangKy);
        }
    }
}
