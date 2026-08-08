using System.ComponentModel.DataAnnotations;

namespace SistemaControlProyectos.Models
{
    public class ClienteViewModel
    {
        [Display(Name = "Razón social")]
        public string RazonSocial { get; set; } = string.Empty;

        [Display(Name = "Contacto (correo o teléfono)")]
        public string Contacto { get; set; } = string.Empty;
    }
}
