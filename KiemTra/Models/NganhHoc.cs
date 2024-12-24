using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KiemTra.Models
{
    public class NganhHoc
    {
        [Key]
        public string MaNganh { get; set; } = string.Empty;

        [Required]
        public string TenNganh { get; set; } = string.Empty;

        public virtual ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
    }
}

