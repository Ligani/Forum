using DataAccess.Contexts;
using DataAccess.Repositories;
using Logics.Interfaces;
using Logics.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Account/Index";
        }
);

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<PostRepository>();

builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}");

    app.Run();