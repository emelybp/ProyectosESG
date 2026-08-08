using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Authorization;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Controllers
{
    // RF-12: solo Directivos, Jefes de Departamento e Ingenieros de Proyecto
    // tienen permiso sobre el módulo "CostosIngresos" (ver PermisoService).
    [RequiereModulo("CostosIngresos")]
    public class CostosIngresosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly CostoIngresoService _service;
        private readonly ReporteService _reporteService;

        public CostosIngresosController(AppDbContext context, CostoIngresoService service, ReporteService reporteService)
        {
            _context = context;
            _service = service;
            _reporteService = reporteService;
        }

        // Muestra los movimientos de un proyecto junto con el total de costos, ingresos y margen (RF-09)
        public async Task<IActionResult> Index(int proyectoId)
        {
            var movimientos = await _context.CostosIngresos
                .Where(m => m.ProyectoId == proyectoId)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();

            ViewBag.ProyectoId = proyectoId;
            ViewBag.CostoTotal = _reporteService.CalcularCostoTotal(movimientos);
            ViewBag.IngresoTotal = _reporteService.CalcularIngresoTotal(movimientos);
            ViewBag.Margen = _reporteService.CalcularMargen(movimientos);

            return View(movimientos);
        }

        // RF-05 / RF-06: registrar costo o ingreso
        public IActionResult Create(int proyectoId, TipoMovimiento tipo)
        {
            return View(new CostoIngresoViewModel { ProyectoId = proyectoId, Tipo = tipo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CostoIngresoViewModel vm)
        {
            var errores = _service.Validar(vm);
            foreach (var error in errores)
                ModelState.AddModelError(string.Empty, error);

            if (!ModelState.IsValid)
                return View(vm);

            var movimiento = _service.CrearDesdeViewModel(vm);
            _context.CostosIngresos.Add(movimiento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { proyectoId = vm.ProyectoId });
        }
    }
}
