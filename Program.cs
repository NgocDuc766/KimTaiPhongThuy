using KimTaiPhongThuy.DataAccess;
using Microsoft.EntityFrameworkCore;
using KimTaiPhongThuy.Models;
namespace KimTaiPhongThuy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Đọc chuỗi kết nối từ appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("MyCnn");

            // Thêm DbContext vào DI container
            builder.Services.AddDbContext<JewelryStoreContext>(options =>
                options.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<AuthenticationDAO>();

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

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}