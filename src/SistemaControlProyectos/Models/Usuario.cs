namespace SistemaControlProyectos.Models
{
    public enum NombreRol
    {
        Directivo,
        JefeDeDepartamento,
        IngenieroDeProyecto,
        AdministradorDeTI
    }

    public class Rol
    {
        public int Id { get; set; }
        public NombreRol Nombre { get; set; }
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string ContrasenaHash { get; set; } = string.Empty; // nunca texto plano (RNF de seguridad)
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }
}
