using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;
using Xunit;

namespace SistemaControlProyectos.Tests
{
    public class PermisoServiceTests
    {
        [Fact]
        public void TienePermiso_DirectivoAccedeAAdministracion_DevuelveTrue()
        {
            var servicio = new PermisoService();
            Assert.True(servicio.TienePermiso(NombreRol.Directivo, "Administracion"));
        }

        [Fact]
        public void TienePermiso_IngenieroNoAccedeAAdministracion_DevuelveFalse()
        {
            var servicio = new PermisoService();
            Assert.False(servicio.TienePermiso(NombreRol.IngenieroDeProyecto, "Administracion"));
        }

        [Fact]
        public void TienePermiso_IngenieroAccedeAProyectos_DevuelveTrue()
        {
            var servicio = new PermisoService();
            Assert.True(servicio.TienePermiso(NombreRol.IngenieroDeProyecto, "Proyectos"));
        }

        [Fact]
        public void TienePermiso_JefeDepartamentoAccedeAReportes_DevuelveTrue()
        {
            var servicio = new PermisoService();
            Assert.True(servicio.TienePermiso(NombreRol.JefeDeDepartamento, "Reportes"));
        }

        [Fact]
        public void TienePermiso_IngenieroNoAccedeAReportes_DevuelveFalse()
        {
            var servicio = new PermisoService();
            Assert.False(servicio.TienePermiso(NombreRol.IngenieroDeProyecto, "Reportes"));
        }

        [Fact]
        public void TienePermiso_AdministradorDeTINoAccedeAModuloOperativo_DevuelveFalse()
        {
            var servicio = new PermisoService();
            Assert.False(servicio.TienePermiso(NombreRol.AdministradorDeTI, "Proyectos"));
        }
    }
}
