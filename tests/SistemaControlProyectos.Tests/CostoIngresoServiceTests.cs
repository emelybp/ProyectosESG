using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;
using Xunit;

namespace SistemaControlProyectos.Tests
{
    public class CostoIngresoServiceTests
    {
        private CostoIngresoViewModel CrearCostoValido()
        {
            return new CostoIngresoViewModel
            {
                ProyectoId = 1,
                Tipo = TipoMovimiento.Costo,
                Categoria = CategoriaCosto.Materiales,
                Monto = 15000m,
                Fecha = new DateTime(2026, 8, 1)
            };
        }

        private CostoIngresoViewModel CrearIngresoValido()
        {
            return new CostoIngresoViewModel
            {
                ProyectoId = 1,
                Tipo = TipoMovimiento.Ingreso,
                Categoria = CategoriaCosto.Factura,
                Monto = 30000m,
                Fecha = new DateTime(2026, 8, 1),
                FolioFiscal = "A1B2C3D4-0001"
            };
        }

        [Fact]
        public void Validar_CostoValido_NoDevuelveErrores()
        {
            var servicio = new CostoIngresoService();
            var errores = servicio.Validar(CrearCostoValido());
            Assert.Empty(errores);
        }

        [Fact]
        public void Validar_IngresoValidoConFolio_NoDevuelveErrores()
        {
            var servicio = new CostoIngresoService();
            var errores = servicio.Validar(CrearIngresoValido());
            Assert.Empty(errores);
        }

        [Fact]
        public void Validar_MontoCero_DevuelveError()
        {
            var servicio = new CostoIngresoService();
            var vm = CrearCostoValido();
            vm.Monto = 0;

            var errores = servicio.Validar(vm);

            Assert.Contains(errores, e => e.Contains("monto"));
        }

        [Fact]
        public void Validar_IngresoSinFolioFiscal_DevuelveError()
        {
            var servicio = new CostoIngresoService();
            var vm = CrearIngresoValido();
            vm.FolioFiscal = null;

            var errores = servicio.Validar(vm);

            Assert.Contains(errores, e => e.Contains("folio fiscal"));
        }

        [Fact]
        public void Validar_CostoConCategoriaFactura_DevuelveError()
        {
            var servicio = new CostoIngresoService();
            var vm = CrearCostoValido();
            vm.Categoria = CategoriaCosto.Factura;

            var errores = servicio.Validar(vm);

            Assert.Contains(errores, e => e.Contains("Factura"));
        }

        [Fact]
        public void CrearDesdeViewModel_EnCosto_NoConservaFolioFiscal()
        {
            var servicio = new CostoIngresoService();
            var vm = CrearCostoValido();
            vm.FolioFiscal = "no debería guardarse";

            var movimiento = servicio.CrearDesdeViewModel(vm);

            Assert.Null(movimiento.FolioFiscal);
            Assert.Equal(TipoMovimiento.Costo, movimiento.Tipo);
        }
    }
}
