using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Services
{
    // Define qué módulos puede usar cada rol (RF-12, Fase 2 sección 4.1)
    public class PermisoService
    {
        // "Administracion" = alta/edición de usuarios y roles (solo TI y Directivos)
        // "Reportes" = consulta de reportes (Directivos, Jefes de Departamento y TI)
        // "Clientes" = alta y consulta de clientes (los 4 roles)
        // El resto son los módulos operativos del día a día
        private static readonly Dictionary<NombreRol, HashSet<string>> ModulosPorRol = new()
        {
            [NombreRol.Directivo] = new HashSet<string>
                { "Proyectos", "Documentos", "CostosIngresos", "PlanTrabajo", "Reportes", "Administracion", "Clientes" },

            [NombreRol.AdministradorDeTI] = new HashSet<string>
                { "Administracion", "Clientes" },

            [NombreRol.JefeDeDepartamento] = new HashSet<string>
                { "Proyectos", "Documentos", "CostosIngresos", "PlanTrabajo", "Reportes", "Clientes" },

            [NombreRol.IngenieroDeProyecto] = new HashSet<string>
                { "Proyectos", "Documentos", "CostosIngresos", "PlanTrabajo", "Clientes" },
        };

        public bool TienePermiso(NombreRol rol, string modulo)
        {
            return ModulosPorRol.TryGetValue(rol, out var modulos) && modulos.Contains(modulo);
        }

        public IReadOnlySet<string> ModulosDisponiblesPara(NombreRol rol)
        {
            return ModulosPorRol.TryGetValue(rol, out var modulos) ? modulos : new HashSet<string>();
        }
    }
}
