namespace SistemaControlProyectos.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
        public string Contacto { get; set; } = string.Empty;

        public ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
    }
}
