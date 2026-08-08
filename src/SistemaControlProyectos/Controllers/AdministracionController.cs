using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Authorization;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Controllers
{
    // Módulo "Administracion": Directivo y Administrador de TI (RF-12)
    [RequiereModulo("Administracion")]
    public class AdministracionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly AuditoriaService _auditoriaService;

        public AdministracionController(AppDbContext context, PasswordService passwordService, AuditoriaService auditoriaService)
        {
            _context = context;
            _passwordService = passwordService;
            _auditoriaService = auditoriaService;
        }

        public IActionResult Index() => RedirectToAction(nameof(Usuarios));

        // Alta y consulta de usuarios
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .OrderBy(u => u.NombreCompleto)
                .ToListAsync();
            return View(usuarios);
        }

        public async Task<IActionResult> CrearUsuario()
        {
            await CargarRolesEnViewBag();
            return View(new UsuarioViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearUsuario(UsuarioViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.NombreCompleto))
                ModelState.AddModelError(string.Empty, "El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(vm.Correo))
                ModelState.AddModelError(string.Empty, "El correo es obligatorio.");
            if (string.IsNullOrWhiteSpace(vm.Contrasena) || vm.Contrasena.Length < 8)
                ModelState.AddModelError(string.Empty, "La contraseña debe tener al menos 8 caracteres.");
            if (vm.RolId <= 0)
                ModelState.AddModelError(string.Empty, "Debe seleccionar un rol.");

            if (!ModelState.IsValid)
            {
                await CargarRolesEnViewBag(vm.RolId);
                return View(vm);
            }

            var usuario = new Usuario { NombreCompleto = vm.NombreCompleto, Correo = vm.Correo, RolId = vm.RolId };
            usuario.ContrasenaHash = _passwordService.HashPassword(usuario, vm.Contrasena);
            _context.Usuarios.Add(usuario);

            _context.BitacoraAuditorias.Add(
                _auditoriaService.RegistrarAccion(User.Identity?.Name ?? "Sistema", "Alta", "Usuario"));

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Usuarios));
        }

        // Bitácora de auditoría (solo lectura)
        public async Task<IActionResult> Bitacora()
        {
            var registros = await _context.BitacoraAuditorias
                .OrderByDescending(b => b.Fecha)
                .ToListAsync();
            return View(registros);
        }

        // Mantenimiento de documentos: consulta y eliminación entre todos los proyectos
        public async Task<IActionResult> Documentos()
        {
            var documentos = await _context.Documentos
                .Include(d => d.Proyecto)
                .OrderByDescending(d => d.FechaCarga)
                .ToListAsync();
            return View(documentos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDocumento(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento != null)
            {
                _context.Documentos.Remove(documento);
                _context.BitacoraAuditorias.Add(
                    _auditoriaService.RegistrarAccion(User.Identity?.Name ?? "Sistema", "Eliminación", "Documento"));
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Documentos));
        }

        private async Task CargarRolesEnViewBag(int? seleccionado = null)
        {
            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Nombre", seleccionado);
        }
    }
}
