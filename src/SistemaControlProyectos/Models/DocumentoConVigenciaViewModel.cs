namespace SistemaControlProyectos.Models
{
    // Envuelve un documento junto con sus banderas de vigencia calculadas (RE-03)
    public class DocumentoConVigenciaViewModel
    {
        public Documento Documento { get; set; } = null!;
        public bool Vencido { get; set; }
        public bool PorVencer { get; set; }
    }
}
