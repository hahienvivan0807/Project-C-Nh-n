using Microsoft.AspNetCore.Mvc;
using Project_Cá_Nhân.Data;
using Project_Cá_Nhân.Models;

namespace Project_Cá_Nhân.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleAuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SimpleAuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class DangKyRequest
        {
            public string HoTen { get; set; }
            public string SoDienThoai { get; set; }
            public string MatKhau { get; set; }
        }


        [HttpPost("dangky")]
        public IActionResult DangKy([FromBody] DangKyRequest dulieu)
        {
            if (string.IsNullOrEmpty(dulieu.SoDienThoai) || string.IsNullOrEmpty(dulieu.MatKhau) || string.IsNullOrEmpty(dulieu.HoTen))
            {
                return BadRequest(new { message = "Vui lòng nhập đủ thông tin!" });
            }

            var khachCu = _context.Users
                .FirstOrDefault(k => k.SoDienThoai == dulieu.SoDienThoai);

            if (khachCu != null)
            {
                return BadRequest(new { message = "Số điện thoại này đã được đăng ký!" });
            }

            var khachMoi = new Users
            {
                HoTen = dulieu.HoTen,
                SoDienThoai = dulieu.SoDienThoai,
                MatKhau = dulieu.MatKhau,
                DiemUyTin = 100,
                ChucVu = "Customer"
            };
            _context.Users.Add(khachMoi);
            _context.SaveChanges();

            return Ok(new { message = "Đăng ký thành công! Bạn có thể đăng nhập ngay." });
        } 
        public class DangNhapRequest
        {
            public string SoDienThoai { get; set; }
            public string MatKhau { get; set; }
        }
        [HttpPost("dangnhap")]
        public IActionResult DangNhap([FromBody] DangNhapRequest dulieu)
        {
            if (string.IsNullOrEmpty(dulieu.SoDienThoai) || string.IsNullOrEmpty(dulieu.MatKhau))
            {
                return BadRequest(new { message = "Vui lòng nhập đủ thông tin!" });
            }
            var khach = _context.Users.FirstOrDefault(k => k.SoDienThoai == dulieu.SoDienThoai);
            if (khach == null || khach.MatKhau != dulieu.MatKhau)
            {
                return BadRequest(new { message = "Số điện thoại hoặc mật khẩu không đúng" });
            }
            if (khach.SoDienThoai == dulieu.SoDienThoai && khach.ChucVu == "employee")
            {
                return Ok(new { message = "Đăng nhập thành công!", HoTen = khach.HoTen, ChucVu = khach.ChucVu });
            }
            return Ok(new { message = "Đăng nhập thành công!", HoTen = khach.HoTen});
        }
        public class DoiMatKhauRequest
        {
            public string SoDienThoai { get; set; }
            public string MatKhauCu { get; set; }
            public string MatKhauMoi { get; set; }
            public string XacNhanMatKhauMoi { get; set; }
        }
        [HttpPost("doimatkhau")]
        public IActionResult DoiMatKhau([FromBody] DoiMatKhauRequest dulieu)
        {
            if (string.IsNullOrEmpty(dulieu.SoDienThoai) || string.IsNullOrEmpty(dulieu.MatKhauCu) || string.IsNullOrEmpty(dulieu.MatKhauMoi))
            {
                return BadRequest(new { message = "Vui lòng nhập đầy đủ thông tin" });
            }
            var khach = _context.Users.FirstOrDefault(k => k.SoDienThoai == dulieu.SoDienThoai);
            if (khach == null)
            {
                return BadRequest(new { message = "Không tìm thấy khách hàng!" });
            }
            if (khach.MatKhau != dulieu.MatKhauCu)
            {
                return BadRequest(new { message = "Số điện thoại hoặc Mật khẩu cũ không đúng!" });
            }
            if (dulieu.MatKhauMoi != dulieu.XacNhanMatKhauMoi)
            {
                return BadRequest(new { message = "Mật khẩu mới không khớp!" });
            }
            khach.MatKhau = dulieu.MatKhauMoi;
            _context.SaveChanges();
            return Ok(new { message = "Đổi mật khẩu thành công !", });

        }

    }
}