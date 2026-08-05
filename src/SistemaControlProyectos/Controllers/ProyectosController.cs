using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ProyectoService _service;

        public ProyectosController(AppDbContext context, ProyectoService service)
        {
            _context = context;
            _service = service;
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

        // RF-01: registrar proyecto
        public IActionResult Create()
        {
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
                return View(vm);

            var proyecto = _service.CrearDesdeViewModel(vm);
            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // RF-02: editar proyecto y cambiar su estatus
        public async Task<IActionResult> Edit(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
                return NotFound();

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
                return View(vm);

            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
                return NotFound();

            _service.ActualizarEntidad(proyecto, vm);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
