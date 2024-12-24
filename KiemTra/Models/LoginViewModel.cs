using System.ComponentModel.DataAnnotations;

namespace KiemTra.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã số sinh viên")]
        [Display(Name = "Mã số sinh viên")]
        public string MSSV { get; set; } = string.Empty;

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }
}