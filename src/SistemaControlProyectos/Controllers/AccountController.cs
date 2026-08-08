using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Controllers
{
    // RF-11: inicio de sesión real con cookies de autenticación
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly AuditoriaService _auditoriaService;

        public AccountController(AppDbContext context, PasswordService passwordService, AuditoriaService auditoriaService)
        {
            _context = context;
            _passwordService = passwordService;
            _auditoriaService = auditoriaService;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == vm.Correo);

            if (usuario == null || usuario.Rol == null ||
                !_passwordService.VerificarPassword(usuario, usuario.ContrasenaHash, vm.Contrasena))
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim("Rol", usuario.Rol.Nombre.ToString()) // RF-12: usado por RequiereModuloAttribute
            };
            var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad));

            _context.BitacoraAuditorias.Add(
                _auditoriaService.RegistrarAccion(usuario.Correo, "Inicio de sesión", "Usuario"));
            await _context.SaveChangesAsync();

            // El Administrador de TI no opera proyectos: se le manda directo a Administración
            if (usuario.Rol.Nombre == NombreRol.AdministradorDeTI)
                return RedirectToAction("Usuarios", "Administracion");

            return RedirectToAction("Index", "Proyectos");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
