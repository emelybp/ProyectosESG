using Microsoft.AspNetCore.Http;
using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Models
{
    // Datos capturados al cargar un documento a un proyecto (RF-03)
    public class DocumentoViewModel
    {
        public int ProyectoId { get; set; }
        public TipoDocumento Tipo { get; set; }
        public IFormFile? Archivo { get; set; }
        public DateTime? FechaVigencia { get; set; } // RE-03: vigencia de permisos/fianzas
    }
}
