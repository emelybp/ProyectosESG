using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;
using Xunit;

namespace SistemaControlProyectos.Tests
{
    public class ProyectoServiceTests
    {
        private ProyectoViewModel CrearViewModelValido()
        {
            return new ProyectoViewModel
            {
                Nombre = "Planta solar Torreón",
                ClienteId = 1,
                TipoSistema = "Paneles solares",
                FechaInicio = new DateTime(2026, 8, 1),
                FechaFinEstimada = new DateTime(2026, 10, 1)
            };
        }

        [Fact]
        public void Validar_ConDatosCompletos_NoDevuelveErrores()
        {
            var servicio = new ProyectoService();
            var errores = servicio.Validar(CrearViewModelValido());
            Assert.Empty(errores);
        }

        [Fact]
        public void Validar_SinNombre_DevuelveError()
        {
            var servicio = new ProyectoService();
            var vm = CrearViewModelValido();
            vm.Nombre = "";

            var errores = servicio.Validar(vm);

            Assert.Contains(errores, e => e.Contains("nombre"));
        }

        [Fact]
        public void Validar_SinCliente_DevuelveError()
        {
            var servicio = new ProyectoService();
            var vm = CrearViewModelValido();
            vm.ClienteId = 0;

            var errores = servicio.Validar(vm);

            Assert.Contains(errores, e => e.Contains("cliente"));
        }

        [Fact]
        public void Validar_FechaFinAnteriorAInicio_DevuelveError()
        {
            var servicio = new ProyectoService();
            var vm = CrearViewModelValido();
            vm.FechaFinEstimada = new DateTime(2026, 7, 1); // antes que FechaInicio

            var errores = servicio.Validar(vm);

            Assert.Contains(errores, e => e.Contains("fecha de fin"));
        }

        [Fact]
        public void CrearDesdeViewModel_AsignaEstatusInicialEnPropuesta()
        {
            var servicio = new ProyectoService();
            var proyecto = servicio.CrearDesdeViewModel(CrearViewModelValido());

            Assert.Equal(EstatusProyecto.EnPropuesta, proyecto.Estatus);
            Assert.Equal("Planta solar Torreón", proyecto.Nombre);
        }
    }
}
