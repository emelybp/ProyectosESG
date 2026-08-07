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

        [Fact]
        public void ValidarCambioEstatus_CompletarTareaQueNoHaIniciado_DevuelveError()
        {
            var servicio = new TareaService();
            var tarea = new Tarea
            {
                FechaInicio = DateTime.Today.AddDays(10), // inicia en el futuro
                FechaFin = DateTime.Today.AddDays(20),
                Estatus = EstatusTarea.Pendiente
            };

            var errores = servicio.ValidarCambioEstatus(tarea, EstatusTarea.Completada);

            Assert.Contains(errores, e => e.Contains("no ha iniciado"));
        }

        [Fact]
        public void ValidarCambioEstatus_CompletarTareaYaIniciada_NoDevuelveErrores()
        {
            var servicio = new TareaService();
            var tarea = new Tarea
            {
                FechaInicio = DateTime.Today.AddDays(-5), // ya inició
                FechaFin = DateTime.Today.AddDays(5),
                Estatus = EstatusTarea.EnProceso
            };

            var errores = servicio.ValidarCambioEstatus(tarea, EstatusTarea.Completada);

            Assert.Empty(errores);
        }

        [Fact]
        public void EstaVencida_FechaFinPasadaYNoCompletada_DevuelveTrue()
        {
            var servicio = new TareaService();
            var tarea = new Tarea
            {
                FechaInicio = new DateTime(2020, 1, 1),
                FechaFin = new DateTime(2020, 1, 10), // muy en el pasado
                Estatus = EstatusTarea.EnProceso
            };

            Assert.True(servicio.EstaVencida(tarea));
        }

        [Fact]
        public void EstaVencida_FechaFinPasadaPeroCompletada_DevuelveFalse()
        {
            var servicio = new TareaService();
            var tarea = new Tarea
            {
                FechaInicio = new DateTime(2020, 1, 1),
                FechaFin = new DateTime(2020, 1, 10),
                Estatus = EstatusTarea.Completada
            };

            Assert.False(servicio.EstaVencida(tarea));
        }

        [Fact]
        public void EstaVencida_FechaFinFutura_DevuelveFalse()
        {
            var servicio = new TareaService();
            var tarea = new Tarea
            {
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(30),
                Estatus = EstatusTarea.Pendiente
            };

            Assert.False(servicio.EstaVencida(tarea));
        }
    }
}
