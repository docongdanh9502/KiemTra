using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiemTra.Models
{
    public class HocPhan
    {
        [Key]
        [Required(ErrorMessage = "Mã học phần là bắt buộc")]
        [Display(Name = "Mã học phần")]
        public string MaHP { get; set; } = null!;

        [Required(ErrorMessage = "Tên học phần là bắt buộc")]
        [Display(Name = "Tên học phần")]
        public string TenHP { get; set; } = null!;

        [Required(ErrorMessage = "Số tín chỉ là bắt buộc")]
        [Range(1, 10, ErrorMessage = "Số tín chỉ phải từ 1 đến 10")]
        [Display(Name = "Số tín chỉ")]
        public int SoTinChi { get; set; }

        [Required]
        [Display(Name = "Số lượng dự kiến")]
        public int SoLuongDuKien { get; set; }

        [Display(Name = "Số lượng đã đăng ký")]
        public int SoLuongDaDangKy { get; set; }

        public virtual ICollection<ChiTietDangKy>? ChiTietDangKys { get; set; }
    }
}