using Lec1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//Getting Connection String
string connString = builder.Configuration.GetConnectionString("DefaultConnection");
//Getting Assembly Name
var migrationAssembly = typeof(Program).Assembly.GetName().Name;


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(connString, sql => sql.MigrationsAssembly(migrationAssembly))

           );

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
