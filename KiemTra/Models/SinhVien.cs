using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace KiemTra.Models
{
    public class SinhVien
    {
        [Key]
        [Required]
        [StringLength(50)]
        [Display(Name = "Mã sinh viên")]
        public string MaSV { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = null!;

        [Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? Hinh { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã ngành")]
        public string MaNganh { get; set; } = null!;

        [ForeignKey("MaNganh")]
        public NganhHoc? NganhHoc { get; set; }

        [NotMapped]
        [Display(Name = "Upload hình")]
        public IFormFile? ImageFile { get; set; }

        public virtual ICollection<DangKy>? DangKys { get; set; }
    }
}

