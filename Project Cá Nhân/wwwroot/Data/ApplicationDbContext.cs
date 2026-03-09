using Microsoft.EntityFrameworkCore;
using Project_Cá_Nhân.Models;
namespace Project_Cá_Nhân.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HangHoa> HangHoa { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
    }
}