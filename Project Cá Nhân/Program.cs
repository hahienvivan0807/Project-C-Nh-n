using Microsoft.EntityFrameworkCore;
using Project_Cá_Nhân.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Kết nối Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Cấu hình CORS (Cho phép gọi API từ bên ngoài)
builder.Services.AddCors(options =>
{
    options.AddPolicy("ChoPhepTatCa",
        policy =>
        {
            policy.WithOrigins("*")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// 3. Đăng ký các dịch vụ (Services)
builder.Services.AddRazorPages();
builder.Services.AddControllers(); //Để chạy được API Login/Register

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 4. Kích hoạt Static Files (CSS, JS, Hình ảnh trong wwwroot)
app.UseStaticFiles(); // <--- SỬA LẠI: Dùng lệnh này chuẩn hơn cho .NET 8

app.UseRouting();

// 5. Kích hoạt CORS (Phải đặt giữa UseRouting và UseAuthorization)
app.UseCors("ChoPhepTatCa"); // <--- MỚI THÊM: Kích hoạt chính sách đã tạo ở trên

app.UseAuthorization();

// 6. Định tuyến (Map routes)
app.MapRazorPages();
app.MapControllers(); // API sẽ chạy qua đường dẫn này

app.Run();