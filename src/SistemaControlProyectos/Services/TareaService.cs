using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Services
{
    // Lógica de negocio para el plan de trabajo (RF-07, RF-08, Fase 2 sección 5.1)
    public class TareaService
    {
        public List<string> Validar(TareaViewModel vm)
        {
            var errores = new List<string>();

            if (vm.ProyectoId <= 0)
                errores.Add("La tarea debe estar asociada a un proyecto.");

            if (string.IsNullOrWhiteSpace(vm.Nombre))
                errores.Add("El nombre de la tarea es obligatorio.");

            if (string.IsNullOrWhiteSpace(vm.Responsable))
                errores.Add("Debe indicar un responsable para la tarea.");

            if (vm.FechaFin < vm.FechaInicio)
                errores.Add("La fecha de fin no puede ser anterior a la fecha de inicio.");

            return errores;
        }

        public Tarea CrearDesdeViewModel(TareaViewModel vm)
        {
            return new Tarea
            {
                ProyectoId = vm.ProyectoId,
                Nombre = vm.Nombre,
                Responsable = vm.Responsable,
                FechaInicio = vm.FechaInicio,
                FechaFin = vm.FechaFin,
                EsHito = vm.EsHito,
                Estatus = EstatusTarea.Pendiente // toda tarea nueva inicia como pendiente
            };
        }

        // RF-08: valida que el cambio de estatus tenga sentido antes de aplicarlo
        public List<string> ValidarCambioEstatus(Tarea tarea, EstatusTarea nuevoEstatus)
        {
            var errores = new List<string>();

            if (nuevoEstatus == EstatusTarea.Completada && tarea.FechaInicio.Date > DateTime.Today)
                errores.Add("No se puede marcar como completada una tarea que aún no ha iniciado.");

            return errores;
        }

        public void ActualizarEstatus(Tarea tarea, EstatusTarea nuevoEstatus)
        {
            tarea.Estatus = nuevoEstatus;
        }

        // RF-08 / RF-13: una tarea está vencida si ya pasó su fecha de fin y no se completó
        public bool EstaVencida(Tarea tarea)
        {
            return tarea.Estatus != EstatusTarea.Completada && tarea.FechaFin.Date < DateTime.Today;
        }
    }
}
