using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Services
{
    // Lógica de negocio para el alta y edición de proyectos (RF-01, RF-02, Fase 2 sección 5.1)
    public class ProyectoService
    {
        public List<string> Validar(ProyectoViewModel vm)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(vm.Nombre))
                errores.Add("El nombre del proyecto es obligatorio.");

            if (vm.ClienteId <= 0)
                errores.Add("Debe seleccionar un cliente.");

            if (string.IsNullOrWhiteSpace(vm.TipoSistema))
                errores.Add("Debe indicar el tipo de sistema a instalar.");

            if (vm.FechaInicio == default)
                errores.Add("La fecha de inicio es obligatoria.");

            if (vm.FechaFinEstimada.HasValue && vm.FechaFinEstimada.Value < vm.FechaInicio)
                errores.Add("La fecha de fin estimada no puede ser anterior a la fecha de inicio.");

            return errores;
        }

        public Proyecto CrearDesdeViewModel(ProyectoViewModel vm)
        {
            return new Proyecto
            {
                Nombre = vm.Nombre,
                ClienteId = vm.ClienteId,
                TipoSistema = vm.TipoSistema,
                FechaInicio = vm.FechaInicio,
                FechaFinEstimada = vm.FechaFinEstimada,
                Estatus = EstatusProyecto.EnPropuesta // todo proyecto nuevo inicia en propuesta
            };
        }

        // RF-02: aplica los cambios del formulario de edición sobre un proyecto ya existente
        public void ActualizarEntidad(Proyecto proyecto, ProyectoViewModel vm)
        {
            proyecto.Nombre = vm.Nombre;
            proyecto.ClienteId = vm.ClienteId;
            proyecto.TipoSistema = vm.TipoSistema;
            proyecto.FechaInicio = vm.FechaInicio;
            proyecto.FechaFinEstimada = vm.FechaFinEstimada;
            proyecto.Estatus = vm.Estatus;
        }

        // RF-02: prepara el formulario de edición con los datos actuales del proyecto
        public ProyectoViewModel AProyectoViewModel(Proyecto proyecto)
        {
            return new ProyectoViewModel
            {
                Id = proyecto.Id,
                Nombre = proyecto.Nombre,
                ClienteId = proyecto.ClienteId,
                TipoSistema = proyecto.TipoSistema,
                FechaInicio = proyecto.FechaInicio,
                FechaFinEstimada = proyecto.FechaFinEstimada,
                Estatus = proyecto.Estatus
            };
        }
    }
}
