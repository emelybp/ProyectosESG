using System.ComponentModel.DataAnnotations;

namespace SistemaControlProyectos.Models
{
    public class UsuarioViewModel
    {
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Display(Name = "Correo")]
        public string Correo { get; set; } = string.Empty;

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;

        [Display(Name = "Rol")]
        public int RolId { get; set; }
    }
}
