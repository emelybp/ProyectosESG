using Microsoft.EntityFrameworkCore;
using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Data
{
    // Contexto de base de datos (Fase 2, sección 3.4 - Modelo de datos preliminar)
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Proyecto> Proyectos => Set<Proyecto>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Documento> Documentos => Set<Documento>();
        public DbSet<CostoIngreso> CostosIngresos => Set<CostoIngreso>();
        public DbSet<Tarea> Tareas => Set<Tarea>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<BitacoraAuditoria> BitacoraAuditorias => Set<BitacoraAuditoria>(); // RNF seguridad

        // RNF rendimiento/escalabilidad: índices sobre las columnas más consultadas
        // en los listados y reportes (Fase 2 sección 5.6)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Documento>().HasIndex(d => d.ProyectoId);
            modelBuilder.Entity<CostoIngreso>().HasIndex(c => c.ProyectoId);
            modelBuilder.Entity<CostoIngreso>().HasIndex(c => c.Fecha);
            modelBuilder.Entity<Tarea>().HasIndex(t => t.ProyectoId);
            modelBuilder.Entity<Tarea>().HasIndex(t => t.FechaFin);
        }
    }
}
