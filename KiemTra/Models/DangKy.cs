using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiemTra.Models
{
    public class DangKy
    {
        [Key]
        public int MaDK { get; set; }

        [Required]
        [StringLength(50)]
        public string MaSV { get; set; } = null!;

        [Required]
        [Display(Name = "Ngày đăng ký")]
        public DateTime NgayDK { get; set; }

        [ForeignKey("MaSV")]
        public virtual SinhVien? SinhVien { get; set; }

        public virtual ICollection<ChiTietDangKy> ChiTietDangKys { get; set; } = new List<ChiTietDangKy>();
    }
}
