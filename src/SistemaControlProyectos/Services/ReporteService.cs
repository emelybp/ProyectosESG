using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Services
{
    // Lógica de los reportes por proyecto (RF-09, RF-13, RF-14, Fase 2 sección 4.3)
    public class ReporteService
    {
        private readonly TareaService _tareaService;

        public ReporteService(TareaService tareaService)
        {
            _tareaService = tareaService;
        }

        // RF-09: costo total, ingreso total y margen
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

        // RF-13: porcentaje de avance del plan de trabajo y tareas vencidas
        public ResultadoAvance CalcularAvance(IEnumerable<Tarea> tareas)
        {
            var lista = tareas.ToList();
            var total = lista.Count;
            var completadas = lista.Count(t => t.Estatus == EstatusTarea.Completada);
            var vencidas = lista.Where(t => _tareaService.EstaVencida(t)).ToList();
            var porcentaje = total == 0 ? 0 : Math.Round((double)completadas / total * 100, 1);

            return new ResultadoAvance
            {
                Completadas = completadas,
                Total = total,
                PorcentajeAvance = porcentaje,
                Vencidas = vencidas
            };
        }

        // RF-14: tipos de documento que aún no se han cargado para el proyecto
        public List<TipoDocumento> DocumentosFaltantes(IEnumerable<Documento> documentosCargados)
        {
            var tiposCargados = documentosCargados.Select(d => d.Tipo).Distinct().ToHashSet();
            return Enum.GetValues<TipoDocumento>()
                .Where(tipo => !tiposCargados.Contains(tipo))
                .ToList();
        }
    }
}
