namespace SistemaControlProyectos.Models
{
    public enum EstatusProyecto
    {
        EnPropuesta,
        EnEjecucion,
        Cerrado
    }

    public class Proyecto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public string TipoSistema { get; set; } = string.Empty; // planta de emergencia, solar, cogeneración, etc.
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinEstimada { get; set; }
        public EstatusProyecto Estatus { get; set; } = EstatusProyecto.EnPropuesta;

        public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
        public ICollection<CostoIngreso> CostosIngresos { get; set; } = new List<CostoIngreso>();
        public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
    }
}
