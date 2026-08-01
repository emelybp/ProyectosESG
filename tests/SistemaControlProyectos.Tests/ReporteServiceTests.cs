using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;
using Xunit;

namespace SistemaControlProyectos.Tests
{
    public class ReporteServiceTests
    {
        [Fact]
        public void CalcularMargen_ConCostosEIngresos_DevuelveDiferenciaCorrecta()
        {
            // Arrange: un proyecto con 2 costos y 2 ingresos de ejemplo (datos ficticios)
            var movimientos = new List<CostoIngreso>
            {
                new CostoIngreso { Tipo = TipoMovimiento.Costo, Categoria = CategoriaCosto.Materiales, Monto = 15000m },
                new CostoIngreso { Tipo = TipoMovimiento.Costo, Categoria = CategoriaCosto.ManoDeObra, Monto = 8000m },
                new CostoIngreso { Tipo = TipoMovimiento.Ingreso, Categoria = CategoriaCosto.Factura, Monto = 30000m },
                new CostoIngreso { Tipo = TipoMovimiento.Ingreso, Categoria = CategoriaCosto.Factura, Monto = 5000m },
            };
            var servicio = new ReporteService();

            // Act
            var costoTotal = servicio.CalcularCostoTotal(movimientos);
            var ingresoTotal = servicio.CalcularIngresoTotal(movimientos);
            var margen = servicio.CalcularMargen(movimientos);

            // Assert
            Assert.Equal(23000m, costoTotal);
            Assert.Equal(35000m, ingresoTotal);
            Assert.Equal(12000m, margen);
        }

        [Fact]
        public void CalcularMargen_SinMovimientos_DevuelveCero()
        {
            var servicio = new ReporteService();
            var margen = servicio.CalcularMargen(new List<CostoIngreso>());
            Assert.Equal(0m, margen);
        }
    }
}
