using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Services
{
    // Lógica del reporte costo-ingreso por proyecto (RF-09, Fase 2 sección 4.3)
    public class ReporteService
    {
        public decimal CalcularCostoTotal(IEnumerable<CostoIngreso> movimientos)
        {
            return movimientos
                .Where(m => m.Tipo == TipoMovimiento.Costo)
                .Sum(m => m.Monto);
        }

        public decimal CalcularIngresoTotal(IEnumerable<CostoIngreso> movimientos)
        {
            return movimientos
                .Where(m => m.Tipo == TipoMovimiento.Ingreso)
                .Sum(m => m.Monto);
        }

        public decimal CalcularMargen(IEnumerable<CostoIngreso> movimientos)
        {
            var lista = movimientos.ToList();
            return CalcularIngresoTotal(lista) - CalcularCostoTotal(lista);
        }
    }
}
