using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Authorization;
using SistemaControlProyectos.Data;
using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Controllers
{
    // Módulo propio "Clientes": los 4 roles tienen acceso (Directivo, Jefe de
    // Departamento, Ingeniero de Proyecto y Administrador de TI)
    [RequiereModulo("Clientes")]
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes.OrderBy(c => c.RazonSocial).ToListAsync();
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View(new ClienteViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.RazonSocial))
                ModelState.AddModelError(string.Empty, "La razón social es obligatoria.");

            if (!ModelState.IsValid)
                return View(vm);

            _context.Clientes.Add(new Cliente { RazonSocial = vm.RazonSocial, Contacto = vm.Contacto });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
