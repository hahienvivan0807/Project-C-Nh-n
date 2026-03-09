using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_Cá_Nhân.Pages
{
    public class IndexModel : PageModel
    {
        public string ThongBao { get; set; }
        public void OnGet()
        {
            ThongBao = "Dữ liệu này được gửi từ Backend C# đó!";
        }
    }
}
