using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Cá_Nhân.Models
{
    [Table("Users")]
    public class Users
    {
        public int Id { get; set; } // Khóa chính
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string MatKhau { get; set; }
        public string ChucVu { get; set; }
        public int? DiemUyTin { get; set; } = 0;
    }
    [Table("HangHoa")]
    public class HangHoa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string TenHang { get; set; }

        [StringLength(500)]
        public string? MoTa { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia { get; set; }

        [Required]
        public bool TrangThai { get; set; } = true;

        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
    [Table("ChiTietDonHang")]
    public class ChiTietDonHang
    {
        [Key]
        public int Id { get; set; }

        public int DonHangId { get; set; }

        public int HangHoaId { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        [ForeignKey("DonHangId")]
        public DonHang DonHang { get; set; }
        [ForeignKey("HangHoaId")]
        public HangHoa HangHoa { get; set; }
    }
    [Table("DonHang")]
    public class DonHang
    {
        [Key]
        public int Id { get; set; }

        public string TenKhach { get; set; }

        public string TrangThai { get; set; }

        public TimeSpan GioNhan { get; set; }

        public decimal TongTien { get; set; }

        public List<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
