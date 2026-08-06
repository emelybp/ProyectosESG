using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;
using Xunit;

namespace SistemaControlProyectos.Tests
{
    public class TareaServiceTests
    {
        private TareaViewModel CrearViewModelValido()
        {
            return new TareaViewModel
            {
                ProyectoId = 1,
                Nombre = "Instalación de paneles",
                Responsable = "Ing. López",
                FechaInicio = new DateTime(2026, 8, 10),
                FechaFin = new DateTime(2026, 8, 20),
                EsHito = false
            };
        }

        [Fact]
        public void Validar_DatosValidos_NoDevuelveErrores()
        {
            var servicio = new TareaService();
            var errores = servicio.Validar(CrearViewModelValido());
            Assert.Empty(errores);
        }

        [Fact]
        public void Validar_SinNombre_DevuelveError()
        {
            var servicio = new TareaService();
            var vm = CrearViewModelValido();
            vm.Nombre = "";

            var errores = servicio.Validar(vm);

            Assert.Contains(errores, e => e.Contains("nombre"));
        }

        [Fact]
        public void Validar_SinResponsable_DevuelveError()
        {
            var servicio = new TareaService();
            var vm = CrearViewModelValido();
            vm.Responsable = "";

            var errores = servicio.Validar(vm);

            Assert.Contains(errores, e => e.Contains("responsable"));
        }

        [Fact]
        public void Validar_FechaFinAnteriorAInicio_DevuelveError()
        {
            var servicio = new TareaService();
            var vm = CrearViewModelValido();
            vm.FechaFin = new DateTime(2026, 8, 1); // antes que FechaInicio

            var errores = servicio.Validar(vm);

            Assert.Contains(errores, e => e.Contains("fecha de fin"));
        }

        [Fact]
        public void CrearDesdeViewModel_AsignaEstatusInicialPendiente()
        {
            var servicio = new TareaService();
            var tarea = servicio.CrearDesdeViewModel(CrearViewModelValido());

            Assert.Equal(EstatusTarea.Pendiente, tarea.Estatus);
            Assert.Equal("Instalación de paneles", tarea.Nombre);
        }
    }
}
