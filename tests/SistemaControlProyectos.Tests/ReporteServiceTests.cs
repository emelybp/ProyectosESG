using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;
using Xunit;

namespace SistemaControlProyectos.Tests
{
    public class ReporteServiceTests
    {
        private ReporteService CrearServicio() => new ReporteService(new TareaService());

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
            var servicio = CrearServicio();

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
            var servicio = CrearServicio();
            var margen = servicio.CalcularMargen(new List<CostoIngreso>());
            Assert.Equal(0m, margen);
        }

        [Fact]
        public void CalcularAvance_ConTareasMixtas_CalculaPorcentajeYVencidasCorrectamente()
        {
            var servicio = CrearServicio();
            var tareas = new List<Tarea>
            {
                new Tarea { Nombre = "A", FechaInicio = new DateTime(2020,1,1), FechaFin = new DateTime(2020,1,10), Estatus = EstatusTarea.Completada },
                new Tarea { Nombre = "B", FechaInicio = new DateTime(2020,1,1), FechaFin = new DateTime(2020,1,10), Estatus = EstatusTarea.EnProceso }, // vencida
                new Tarea { Nombre = "C", FechaInicio = DateTime.Today, FechaFin = DateTime.Today.AddDays(30), Estatus = EstatusTarea.Pendiente },
                new Tarea { Nombre = "D", FechaInicio = DateTime.Today, FechaFin = DateTime.Today.AddDays(30), Estatus = EstatusTarea.Completada },
            };

            var resultado = servicio.CalcularAvance(tareas);

            Assert.Equal(4, resultado.Total);
            Assert.Equal(2, resultado.Completadas);
            Assert.Equal(50.0, resultado.PorcentajeAvance);
            Assert.Single(resultado.Vencidas);
            Assert.Equal("B", resultado.Vencidas[0].Nombre);
        }

        [Fact]
        public void CalcularAvance_SinTareas_DevuelveCeroSinError()
        {
            var servicio = CrearServicio();
            var resultado = servicio.CalcularAvance(new List<Tarea>());

            Assert.Equal(0, resultado.Total);
            Assert.Equal(0, resultado.Completadas);
            Assert.Equal(0, resultado.PorcentajeAvance);
            Assert.Empty(resultado.Vencidas);
        }

        [Fact]
        public void DocumentosFaltantes_ProyectoSinDocumentos_DevuelveTodosLosTipos()
        {
            var servicio = CrearServicio();
            var faltantes = servicio.DocumentosFaltantes(new List<Documento>());

            Assert.Equal(Enum.GetValues<TipoDocumento>().Length, faltantes.Count);
        }

        [Fact]
        public void DocumentosFaltantes_ProyectoConAlgunosDocumentos_DevuelveSoloLosFaltantes()
        {
            var servicio = CrearServicio();
            var documentos = new List<Documento>
            {
                new Documento { Tipo = TipoDocumento.Contrato },
                new Documento { Tipo = TipoDocumento.Plano },
            };

            var faltantes = servicio.DocumentosFaltantes(documentos);

            Assert.DoesNotContain(TipoDocumento.Contrato, faltantes);
            Assert.DoesNotContain(TipoDocumento.Plano, faltantes);
            Assert.Contains(TipoDocumento.Fianza, faltantes);
        }
    }
}
