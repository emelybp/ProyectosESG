namespace SistemaControlProyectos.Models
{
    // Envuelve una tarea junto con su indicador calculado de "vencida" (RF-08)
    public class TareaConEstatusViewModel
    {
        public Tarea Tarea { get; set; } = null!;
        public bool Vencida { get; set; }
    }
}
