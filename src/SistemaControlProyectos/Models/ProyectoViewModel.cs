using System.ComponentModel.DataAnnotations;

namespace SistemaControlProyectos.Models
{
    // Datos que se capturan en el formulario de alta/edición de proyecto (RF-01, RF-02)
    public class ProyectoViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre del proyecto")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Display(Name = "Tipo de sistema")]
        public string TipoSistema { get; set; } = string.Empty; // planta de emergencia, solar, cogeneración, etc.

        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [Display(Name = "Fecha de fin estimada")]
        [DataType(DataType.Date)]
        public DateTime? FechaFinEstimada { get; set; }
    }
}
