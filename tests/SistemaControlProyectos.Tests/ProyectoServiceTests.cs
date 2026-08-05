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

        [Fact]
        public void ActualizarEntidad_ModificaCamposDelProyectoExistente()
        {
            var servicio = new ProyectoService();
            var proyecto = new Proyecto
            {
                Id = 5,
                Nombre = "Nombre viejo",
                ClienteId = 1,
                TipoSistema = "Solar",
                FechaInicio = new DateTime(2026, 1, 1),
                Estatus = EstatusProyecto.EnPropuesta
            };
            var vm = new ProyectoViewModel
            {
                Nombre = "Nombre actualizado",
                ClienteId = 2,
                TipoSistema = "Cogeneración",
                FechaInicio = new DateTime(2026, 2, 1),
                Estatus = EstatusProyecto.EnEjecucion
            };

            servicio.ActualizarEntidad(proyecto, vm);

            Assert.Equal("Nombre actualizado", proyecto.Nombre);
            Assert.Equal(EstatusProyecto.EnEjecucion, proyecto.Estatus);
            Assert.Equal(5, proyecto.Id); // el Id nunca debe cambiar al editar
        }

        [Fact]
        public void AProyectoViewModel_MapeaCamposCorrectamente()
        {
            var servicio = new ProyectoService();
            var proyecto = new Proyecto
            {
                Id = 7,
                Nombre = "Biodigestor Norte",
                ClienteId = 3,
                TipoSistema = "Biodigestor",
                FechaInicio = new DateTime(2026, 3, 1),
                Estatus = EstatusProyecto.Cerrado
            };

            var vm = servicio.AProyectoViewModel(proyecto);

            Assert.Equal(7, vm.Id);
            Assert.Equal("Biodigestor Norte", vm.Nombre);
            Assert.Equal(EstatusProyecto.Cerrado, vm.Estatus);
        }
    }
}
