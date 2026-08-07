using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Authorization;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Controllers
{
    // RF-12: módulo "Documentos" disponible para Directivo, Jefe de Departamento e Ingeniero de Proyecto
    [RequiereModulo("Documentos")]
    public class DocumentosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly DocumentoService _service;
        private readonly IWebHostEnvironment _entorno;

        public DocumentosController(AppDbContext context, DocumentoService service, IWebHostEnvironment entorno)
        {
            _context = context;
            _service = service;
            _entorno = entorno;
        }

        // RF-04: consultar los documentos de un proyecto, con su estatus de vigencia (RE-03)
        public async Task<IActionResult> Index(int proyectoId)
        {
            var documentos = await _context.Documentos
                .Where(d => d.ProyectoId == proyectoId)
                .OrderByDescending(d => d.FechaCarga)
                .ToListAsync();

            var modelo = documentos
                .Select(d => new DocumentoConVigenciaViewModel
                {
                    Documento = d,
                    Vencido = _service.EstaVencido(d),
                    PorVencer = _service.EstaPorVencer(d)
                })
                .ToList();

            ViewBag.ProyectoId = proyectoId;
            return View(modelo);
        }

        // RF-03: cargar documento
        public IActionResult Create(int proyectoId)
        {
            return View(new DocumentoViewModel { ProyectoId = proyectoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentoViewModel vm)
        {
            var errores = _service.Validar(vm);
            foreach (var error in errores)
                ModelState.AddModelError(string.Empty, error);

            if (!ModelState.IsValid)
                return View(vm);

            // Guarda el archivo físico en wwwroot/uploads/{proyectoId}/
            var nombreUnico = _service.GenerarNombreUnico(vm.Archivo!.FileName);
            var carpetaProyecto = Path.Combine(_entorno.WebRootPath, "uploads", vm.ProyectoId.ToString());
            Directory.CreateDirectory(carpetaProyecto);
            var rutaFisica = Path.Combine(carpetaProyecto, nombreUnico);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await vm.Archivo.CopyToAsync(stream);
            }

            var rutaRelativa = $"/uploads/{vm.ProyectoId}/{nombreUnico}";
            var documento = _service.CrearDocumento(vm, rutaRelativa);

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { proyectoId = vm.ProyectoId });
        }
    }
}
