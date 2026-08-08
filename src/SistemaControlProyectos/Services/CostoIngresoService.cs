using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Services
{
    // Lógica de negocio para el registro de costos e ingresos (Fase 2, sección 5.1)
    public class CostoIngresoService
    {
        public List<string> Validar(CostoIngresoViewModel vm)
        {
            var errores = new List<string>();

            if (vm.ProyectoId <= 0)
                errores.Add("El movimiento debe estar asociado a un proyecto.");

            if (vm.Monto <= 0)
                errores.Add("El monto debe ser mayor a cero.");

            if (vm.Fecha == default)
                errores.Add("La fecha es obligatoria.");

            // RE-01: los ingresos deben poder asociarse a un folio fiscal (CFDI)
            if (vm.Tipo == TipoMovimiento.Ingreso && string.IsNullOrWhiteSpace(vm.FolioFiscal))
                errores.Add("Debe capturar el folio fiscal (CFDI) del ingreso.");

            // La categoría "Factura" es exclusiva de los ingresos
            if (vm.Tipo == TipoMovimiento.Costo && vm.Categoria == CategoriaCosto.Factura)
                errores.Add("La categoría 'Factura' solo aplica para ingresos.");

            return errores;
        }

        public CostoIngreso CrearDesdeViewModel(CostoIngresoViewModel vm)
        {
            return new CostoIngreso
            {
                ProyectoId = vm.ProyectoId,
                Tipo = vm.Tipo,
                Categoria = vm.Categoria,
                Monto = vm.Monto,
                Fecha = vm.Fecha,
                FolioFiscal = vm.Tipo == TipoMovimiento.Ingreso ? vm.FolioFiscal : null
            };
        }
    }
}
