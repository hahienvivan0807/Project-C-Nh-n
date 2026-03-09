using Microsoft.AspNetCore.Mvc;
using Project_Cá_Nhân.Data;
using Project_Cá_Nhân.Models;
using System;

[ApiController]
[Route("api/[controller]")]
public class DonHangController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DonHangController(ApplicationDbContext context)
    {
        _context = context;
    }
    public class ChiTietRequest
    {
        public int HangHoaId { get; set; }
        public int SoLuong { get; set; }
    }

    public class DonHangRequest{
        public string TenKhach { get; set; }

        public string GioNhan { get; set; }

        public decimal TongTien { get; set; }

        public List<ChiTietRequest> ChiTietDonHangs { get; set; }
    }
    [HttpPost("donhang")]
    public IActionResult TaoDonHang([FromBody] DonHangRequest request)
    {
        if (request == null || request.ChiTietDonHangs == null || !request.ChiTietDonHangs.Any())
            return BadRequest(new { message = "Không có dữ liệu" });

        if (!TimeSpan.TryParse(request.GioNhan, out TimeSpan gioNhan))
            return BadRequest(new { message = "Giờ nhận không hợp lệ" });

        var donHang = new DonHang
        {
            TenKhach = request.TenKhach,
            GioNhan = gioNhan,
            TrangThai = "Chờ xử lý",
            TongTien = 0
        };
        _context.DonHang.Add(donHang);
        _context.SaveChanges();

        decimal tongTien = 0;
        foreach (var item in request.ChiTietDonHangs)
        {
            var hangHoa = _context.HangHoa.Find(item.HangHoaId);
            if (hangHoa == null)
                return BadRequest(new { message = $"Hàng hóa ID {item.HangHoaId} không tồn tại" });

            var chiTiet = new ChiTietDonHang
            {
                DonHangId = donHang.Id,
                HangHoaId = item.HangHoaId,
                SoLuong = item.SoLuong,
                DonGia = hangHoa.Gia
            };
            tongTien += hangHoa.Gia * item.SoLuong;
            _context.ChiTietDonHang.Add(chiTiet);
        }

        // Cập nhật tổng tiền
        donHang.TongTien = tongTien;
        _context.SaveChanges();

        return Ok(new { message = "Đặt hàng thành công" });
    }
    [HttpGet("danhsach")]
    public IActionResult LayDanhSachDonHang()
    {
        var ds = _context.DonHang
            .OrderByDescending(d => d.Id)
            .Select(d => new
            {
                d.Id,
                d.TenKhach,
                GioNhan = d.GioNhan.ToString(),
                d.TrangThai,
                d.TongTien,
                ChiTiet = _context.ChiTietDonHang
                    .Where(ct => ct.DonHangId == d.Id)
                    .Select(ct => new
                    {
                        TenHang = ct.HangHoa.TenHang,
                        ct.SoLuong
                    }).ToList()
            }).ToList();

        return Ok(ds);  
    }
}