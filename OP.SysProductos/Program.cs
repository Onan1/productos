using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OP.SysProductos.DAL;
using OP.SysProductos.BL;
using OfficeOpenXml;
using OP.SysProductos.EN;
using OP.SysProductos.DAL.Implementaciones;

var builder = WebApplication.CreateBuilder(args);

var stringConec = "Server=127.0.0.1;Port=4406;Database=producto;Uid=root;Pwd=monroy11;";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(stringConec, ServerVersion.AutoDetect(stringConec)));

builder.Services.AddScoped<ProductoDAL>();
builder.Services.AddScoped<ProductoBL>();

builder.Services.AddScoped<ProveedorDAL>();
builder.Services.AddScoped<ProveedorBL>();

builder.Services.AddScoped<CompraDAL>();
builder.Services.AddScoped<CompraBL>();

builder.Services.AddScoped<VentaBL>();
builder.Services.AddScoped<VentaDAL>();
builder.Services.AddScoped<DetalleVenta>();

builder.Services.AddScoped<ClienteDAL>();
builder.Services.AddScoped<ClienteBL>();



builder.Services.AddControllersWithViews();
ExcelPackage.LicenseContext = LicenseContext.Commercial;


var app = builder.Build();
IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../wwwroot/Rotativa");
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Producto}/{action=Index}/{id?}");
app.Run();
