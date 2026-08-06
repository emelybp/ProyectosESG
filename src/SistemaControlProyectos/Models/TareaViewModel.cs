using System.ComponentModel.DataAnnotations;

namespace SistemaControlProyectos.Models
{
    // Datos capturados al registrar una tarea o hito del plan de trabajo (RF-07)
    public class TareaViewModel
    {
        public int ProyectoId { get; set; }

        [Display(Name = "Nombre de la tarea")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Responsable")]
        public string Responsable { get; set; } = string.Empty;

        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [Display(Name = "Fecha de fin")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; } = DateTime.Today;

        [Display(Name = "¿Es un hito?")]
        public bool EsHito { get; set; }
    }
}
