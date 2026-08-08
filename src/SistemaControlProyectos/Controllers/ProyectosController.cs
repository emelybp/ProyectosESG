using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Authorization;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Controllers
{
    // RF-12: módulo "Proyectos" disponible para Directivo, Jefe de Departamento e Ingeniero de Proyecto
    [RequiereModulo("Proyectos")]
    public class ProyectosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ProyectoService _service;
        private readonly AuditoriaService _auditoriaService;

        public ProyectosController(AppDbContext context, ProyectoService service, AuditoriaService auditoriaService)
        {
            _context = context;
            _service = service;
            _auditoriaService = auditoriaService;
        }

        // RF-10: listado general de proyectos
        public async Task<IActionResult> Index()
        {
            var proyectos = await _context.Proyectos
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.FechaInicio)
                .ToListAsync();
            return View(proyectos);
        }

        // Pantalla de detalle: punto de entrada a los módulos de un proyecto (Fase 2, IU-02)
        public async Task<IActionResult> Detalle(int id)
        {
            var proyecto = await _context.Proyectos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proyecto == null)
                return NotFound();

            return View(proyecto);
        }

        // RF-01: registrar proyecto
        public async Task<IActionResult> Create()
        {
            await CargarClientesEnViewBag();
            return View(new ProyectoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProyectoViewModel vm)
        {
            var errores = _service.Validar(vm);
            foreach (var error in errores)
                ModelState.AddModelError(string.Empty, error);

            if (!ModelState.IsValid)
            {
                await CargarClientesEnViewBag(vm.ClienteId);
                return View(vm);
            }

            var proyecto = _service.CrearDesdeViewModel(vm);
            _context.Proyectos.Add(proyecto);

            _context.BitacoraAuditorias.Add(
                _auditoriaService.RegistrarAccion(User.Identity?.Name ?? "Sistema", "Alta", "Proyecto"));

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // RF-02: editar proyecto y cambiar su estatus
        public async Task<IActionResult> Edit(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
                return NotFound();

            await CargarClientesEnViewBag(proyecto.ClienteId);
            return View(_service.AProyectoViewModel(proyecto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProyectoViewModel vm)
        {
            var errores = _service.Validar(vm);
            foreach (var error in errores)
                ModelState.AddModelError(string.Empty, error);

            if (!ModelState.IsValid)
            {
                await CargarClientesEnViewBag(vm.ClienteId);
                return View(vm);
            }

            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
                return NotFound();

            _service.ActualizarEntidad(proyecto, vm);

            _context.BitacoraAuditorias.Add(
                _auditoriaService.RegistrarAccion(User.Identity?.Name ?? "Sistema", "Edición", "Proyecto"));

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarClientesEnViewBag(int? clienteSeleccionado = null)
        {
            var clientes = await _context.Clientes.OrderBy(c => c.RazonSocial).ToListAsync();
            ViewBag.Clientes = new SelectList(clientes, "Id", "RazonSocial", clienteSeleccionado);
        }
    }
}
