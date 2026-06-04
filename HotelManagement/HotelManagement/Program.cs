using HotelManagement.InterfacesRepositories;
using HotelManagement.Repositories;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Ket noi database appsettings.json
builder.Services.AddDbContext<HotelDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Cau hinh cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn đến trang đăng nhập
       // options.LogoutPath = "/Account/Logout"; Đường dẫn đến trang đăng xuất
        options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn đến trang từ chối truy cập
        options.Cookie.IsEssential = true;
    });

// Bat session
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
});

builder.Services.AddHttpContextAccessor();

// Dang ky depency injection cho repository
builder.Services.AddScoped<IPhongRepository, PhongRepository>();
builder.Services.AddScoped<ITaikhoanRepository, TaikhoanRepository>();
builder.Services.AddScoped<INhanvienRepository, NhanvienRepository>();
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IKhachhangRepository, KhachhangRepository>();
builder.Services.AddScoped<IRentRepository, RentRepository>();
builder.Services.AddScoped<IPhuthuRepository, PhuthuRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
