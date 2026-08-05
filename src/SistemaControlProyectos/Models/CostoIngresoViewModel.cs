using System.ComponentModel.DataAnnotations;
using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Models
{
    // Datos capturados al registrar un costo o un ingreso de un proyecto (RF-05, RF-06)
    public class CostoIngresoViewModel
    {
        public int ProyectoId { get; set; }

        [Display(Name = "Tipo de movimiento")]
        public TipoMovimiento Tipo { get; set; }

        [Display(Name = "Categoría")]
        public CategoriaCosto Categoria { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Display(Name = "Folio fiscal (CFDI)")]
        public string? FolioFiscal { get; set; } // RE-01: obligatorio solo cuando Tipo = Ingreso
    }
}
