using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Services;

var builder = WebApplication.CreateBuilder(args);

// Base de datos (Fase 2, sección 7 - SQL Server Express)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Autenticación por cookies (RF-11)
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Servicios de negocio (uno por módulo, ver Fase 2 sección 4.2)
builder.Services.AddScoped<ReporteService>();
builder.Services.AddScoped<ProyectoService>();
builder.Services.AddScoped<DocumentoService>();
builder.Services.AddScoped<CostoIngresoService>();
builder.Services.AddScoped<PermisoService>();
builder.Services.AddScoped<TareaService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<AuditoriaService>();

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

// Aplica las migraciones pendientes y carga datos de ejemplo al arrancar.
// Útil para el servidor de pruebas: no hace falta correr comandos aparte.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordService = scope.ServiceProvider.GetRequiredService<PasswordService>();
    await context.Database.MigrateAsync();
    await DbSeeder.SeedAsync(context, passwordService);
}

app.Run();
