using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;      
using WebApplication2.Services;   
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<YouTubeApiService>();


builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();