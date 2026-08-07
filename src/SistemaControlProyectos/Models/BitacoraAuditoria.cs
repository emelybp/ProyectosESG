namespace SistemaControlProyectos.Models
{
    // Registro de auditoría: quién hizo qué y cuándo (RNF de seguridad, Fase 2 sección 5.6.1)
    public class BitacoraAuditoria
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;   // "Alta", "Edición", "Eliminación"
        public string Entidad { get; set; } = string.Empty;  // "Proyecto", "Documento", "CostoIngreso", etc.
        public DateTime Fecha { get; set; }
    }
}
