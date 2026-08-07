namespace SistemaControlProyectos.Models
{
    // Resultado calculado del reporte de avance de un proyecto (RF-13)
    public class ResultadoAvance
    {
        public int Completadas { get; set; }
        public int Total { get; set; }
        public double PorcentajeAvance { get; set; }
        public List<Tarea> Vencidas { get; set; } = new();
    }
}
