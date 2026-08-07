using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Data
{
    // Carga datos de ejemplo (ficticios) al arrancar, si la base de datos está vacía.
    // Útil para el video de demostración y las pruebas en el servidor.
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context, PasswordService passwordService)
        {
            if (!await context.Roles.AnyAsync())
            {
                context.Roles.AddRange(
                    Enum.GetValues<NombreRol>().Select(r => new Rol { Nombre = r }));
                await context.SaveChangesAsync();
            }

            if (!await context.Usuarios.AnyAsync())
            {
                var roles = await context.Roles.ToListAsync();

                var usuariosDemo = new (string Nombre, string Correo, string Contrasena, NombreRol Rol)[]
                {
                    ("Ricardo Hernández Rodríguez", "directivo@esgsa.com.mx", "Directivo#2026", NombreRol.Directivo),
                    ("Ana Gómez", "jefeproyectos@esgsa.com.mx", "JefeDpto#2026", NombreRol.JefeDeDepartamento),
                    ("Luis Torres", "ingeniero@esgsa.com.mx", "Ingeniero#2026", NombreRol.IngenieroDeProyecto),
                    ("Carlos Gustavo Barragán Olivares", "ti@esgsa.com.mx", "AdminTI#2026", NombreRol.AdministradorDeTI),
                };

                foreach (var (nombre, correo, contrasena, rolNombre) in usuariosDemo)
                {
                    var rol = roles.First(r => r.Nombre == rolNombre);
                    var usuario = new Usuario { NombreCompleto = nombre, Correo = correo, RolId = rol.Id };
                    usuario.ContrasenaHash = passwordService.HashPassword(usuario, contrasena);
                    context.Usuarios.Add(usuario);
                }

                await context.SaveChangesAsync();
            }

            if (!await context.Clientes.AnyAsync())
            {
                context.Clientes.Add(new Cliente
                {
                    RazonSocial = "Industrias del Norte S.A. de C.V.",
                    Contacto = "compras@industriasdelnorte-ejemplo.mx"
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
