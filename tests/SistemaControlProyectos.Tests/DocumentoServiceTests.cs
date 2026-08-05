using SistemaControlProyectos.Services;
using Xunit;

namespace SistemaControlProyectos.Tests
{
    public class DocumentoServiceTests
    {
        [Fact]
        public void ValidarArchivo_SinNombre_DevuelveError()
        {
            var servicio = new DocumentoService();

            var errores = servicio.ValidarArchivo(null, 0);

            Assert.Contains(errores, e => e.Contains("seleccionar un archivo"));
        }

        [Fact]
        public void ValidarArchivo_ExtensionNoPermitida_DevuelveError()
        {
            var servicio = new DocumentoService();

            var errores = servicio.ValidarArchivo("instalador.exe", 1000);

            Assert.Contains(errores, e => e.Contains("no permitido"));
        }

        [Fact]
        public void ValidarArchivo_ArchivoValido_NoDevuelveErrores()
        {
            var servicio = new DocumentoService();

            var errores = servicio.ValidarArchivo("contrato_proyecto.pdf", 500_000);

            Assert.Empty(errores);
        }

        [Fact]
        public void ValidarArchivo_ExcedeTamanioMaximo_DevuelveError()
        {
            var servicio = new DocumentoService();
            long once_mb = 11 * 1024 * 1024;

            var errores = servicio.ValidarArchivo("plano.png", once_mb);

            Assert.Contains(errores, e => e.Contains("10 MB"));
        }

        [Fact]
        public void GenerarNombreUnico_ConservaElNombreOriginalYAgregaPrefijo()
        {
            var servicio = new DocumentoService();

            var nombreUnico = servicio.GenerarNombreUnico("ficha_tecnica_inversor.pdf");

            Assert.EndsWith("_ficha_tecnica_inversor.pdf", nombreUnico);
            Assert.True(nombreUnico.Length > "ficha_tecnica_inversor.pdf".Length);
        }
    }
}
