namespace SistemaControlProyectos.Models
{
    public enum TipoDocumento
    {
        Contrato,
        Permiso,
        Plano,
        Fianza,
        OrdenDeCompra,
        Fotografia,
        FichaTecnica,
        Manual
    }

    public class Documento
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public Proyecto? Proyecto { get; set; }
        public TipoDocumento Tipo { get; set; }
        public string RutaArchivo { get; set; } = string.Empty;
        public DateTime FechaCarga { get; set; } = DateTime.Now;
        public DateTime? FechaVigencia { get; set; } // RE-03: vigencia de permisos/fianzas
    }
}
