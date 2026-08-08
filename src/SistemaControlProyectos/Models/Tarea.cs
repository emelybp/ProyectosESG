namespace SistemaControlProyectos.Models
{
    public enum EstatusTarea
    {
        Pendiente,
        EnProceso,
        Completada,
        Vencida
    }

    public class Tarea
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public Proyecto? Proyecto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Responsable { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstatusTarea Estatus { get; set; } = EstatusTarea.Pendiente;
        public bool EsHito { get; set; }
    }
}
