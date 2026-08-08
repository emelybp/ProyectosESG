using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Authorization;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Controllers
{
    // RF-12: módulo "PlanTrabajo" disponible para Directivo, Jefe de Departamento e Ingeniero de Proyecto
    [RequiereModulo("PlanTrabajo")]
    public class TareasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TareaService _service;

        public TareasController(AppDbContext context, TareaService service)
        {
            _context = context;
            _service = service;
        }

        // Plan de trabajo de un proyecto, con indicador de tareas vencidas (RF-08)
        public async Task<IActionResult> Index(int proyectoId)
        {
            var tareas = await _context.Tareas
                .Where(t => t.ProyectoId == proyectoId)
                .OrderBy(t => t.FechaInicio)
                .ToListAsync();

            var modelo = tareas
                .Select(t => new TareaConEstatusViewModel { Tarea = t, Vencida = _service.EstaVencida(t) })
                .ToList();

            ViewBag.ProyectoId = proyectoId;
            return View(modelo);
        }

        // RF-07: registrar tarea/hito
        public IActionResult Create(int proyectoId)
        {
            return View(new TareaViewModel { ProyectoId = proyectoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TareaViewModel vm)
        {
            var errores = _service.Validar(vm);
            foreach (var error in errores)
                ModelState.AddModelError(string.Empty, error);

            if (!ModelState.IsValid)
                return View(vm);

            var tarea = _service.CrearDesdeViewModel(vm);
            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { proyectoId = vm.ProyectoId });
        }

        // RF-08: actualizar el avance (estatus) de una tarea existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarEstatus(int id, EstatusTarea nuevoEstatus)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
                return NotFound();

            var errores = _service.ValidarCambioEstatus(tarea, nuevoEstatus);
            if (errores.Count > 0)
            {
                TempData["ErrorEstatus"] = string.Join(" ", errores);
                return RedirectToAction(nameof(Index), new { proyectoId = tarea.ProyectoId });
            }

            _service.ActualizarEstatus(tarea, nuevoEstatus);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { proyectoId = tarea.ProyectoId });
        }
    }
}
