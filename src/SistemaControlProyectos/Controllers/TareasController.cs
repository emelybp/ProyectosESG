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

        // Plan de trabajo de un proyecto
        public async Task<IActionResult> Index(int proyectoId)
        {
            var tareas = await _context.Tareas
                .Where(t => t.ProyectoId == proyectoId)
                .OrderBy(t => t.FechaInicio)
                .ToListAsync();

            ViewBag.ProyectoId = proyectoId;
            return View(tareas);
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
    }
}
