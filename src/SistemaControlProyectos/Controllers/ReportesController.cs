using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Authorization;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Controllers
{
    // RF-12: módulo "Reportes" disponible para Directivo, Jefe de Departamento y Administrador de TI
    [RequiereModulo("Reportes")]
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ReporteService _reporteService;

        public ReportesController(AppDbContext context, ReporteService reporteService)
        {
            _context = context;
            _reporteService = reporteService;
        }

        // RF-13: reporte de avance del plan de trabajo
        public async Task<IActionResult> Avance(int proyectoId)
        {
            var tareas = await _context.Tareas
                .Where(t => t.ProyectoId == proyectoId)
                .ToListAsync();

            var resultado = _reporteService.CalcularAvance(tareas);
            ViewBag.ProyectoId = proyectoId;
            return View(resultado);
        }

        // RF-14: reporte de documentación faltante
        public async Task<IActionResult> DocumentacionFaltante(int proyectoId)
        {
            var documentos = await _context.Documentos
                .Where(d => d.ProyectoId == proyectoId)
                .ToListAsync();

            var faltantes = _reporteService.DocumentosFaltantes(documentos);
            ViewBag.ProyectoId = proyectoId;
            return View(faltantes);
        }
    }
}
