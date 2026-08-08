namespace SistemaControlProyectos.Models
{
    public enum TipoMovimiento
    {
        Costo,
        Ingreso
    }

    public enum CategoriaCosto
    {
        Materiales,
        ManoDeObra,
        Viaticos,
        Legal,
        Gestion,
        Factura // se usa cuando TipoMovimiento = Ingreso
    }

    public class CostoIngreso
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public Proyecto? Proyecto { get; set; }
        public TipoMovimiento Tipo { get; set; }
        public CategoriaCosto Categoria { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? FolioFiscal { get; set; } // RE-01: folio CFDI cuando es ingreso
    }
}
