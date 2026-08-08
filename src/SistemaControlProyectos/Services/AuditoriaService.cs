using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Services
{
    // Genera los registros de la bitácora de auditoría (Fase 2 sección 5.6.1)
    public class AuditoriaService
    {
        public BitacoraAuditoria RegistrarAccion(string usuario, string accion, string entidad)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("El usuario es obligatorio para registrar una acción en la bitácora.");

            return new BitacoraAuditoria
            {
                Usuario = usuario,
                Accion = accion,
                Entidad = entidad,
                Fecha = DateTime.Now
            };
        }
    }
}
