using SistemaControlProyectos.Services;
using Xunit;

namespace SistemaControlProyectos.Tests
{
    public class AuditoriaServiceTests
    {
        [Fact]
        public void RegistrarAccion_ConDatosValidos_CreaRegistroCorrecto()
        {
            var servicio = new AuditoriaService();

            var registro = servicio.RegistrarAccion("emely@esgsa.com.mx", "Alta", "Proyecto");

            Assert.Equal("emely@esgsa.com.mx", registro.Usuario);
            Assert.Equal("Alta", registro.Accion);
            Assert.Equal("Proyecto", registro.Entidad);
        }

        [Fact]
        public void RegistrarAccion_SinUsuario_LanzaExcepcion()
        {
            var servicio = new AuditoriaService();

            Assert.Throws<ArgumentException>(() => servicio.RegistrarAccion("", "Alta", "Proyecto"));
        }
    }
}
