using KimTaiPhongThuy.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using KimTaiPhongThuy.DataAccess.Service;
using System.Reflection.Emit;
using KimTaiPhongThuy.Models;
namespace KimTaiPhongThuy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Đọc chuỗi kết nối từ appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("MyDB");

            // Thêm DbContext vào DI container
            builder.Services.AddDbContext<JewelryStoreContext>(options =>
                options.UseSqlServer(connectionString));

            //Đăng ký PasswordHasher vào DI container
            builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>(); // Thêm dòng này
            builder.Services.AddSingleton<PasswordHasherService>(); // Đăng ký
            builder.Services.AddSingleton<EmailService>();

            // Đăng ký dịch vụ session vào DI container
            builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ cho cache
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của session
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Sử dụng cookie chỉ trên HTTPS
            });

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<AuthenticationDAO>();
            builder.Services.AddScoped<ProductDAO>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}