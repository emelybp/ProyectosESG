using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Services;

var builder = WebApplication.CreateBuilder(args);

// Base de datos (Fase 2, sección 7 - SQL Server Express)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios de negocio (uno por módulo, ver Fase 2 sección 4.2)
builder.Services.AddScoped<ReporteService>();
builder.Services.AddScoped<ProyectoService>();
builder.Services.AddScoped<DocumentoService>();
builder.Services.AddScoped<CostoIngresoService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Proyectos}/{action=Index}/{id?}");

app.Run();
