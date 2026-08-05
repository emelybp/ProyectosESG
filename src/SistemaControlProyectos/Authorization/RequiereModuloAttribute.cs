using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;

namespace SistemaControlProyectos.Authorization
{
    // Uso: [RequiereModulo("Reportes")] sobre un controlador o una acción.
    // Bloquea el acceso si el rol del usuario autenticado no tiene permiso sobre ese módulo (RF-12).
    public class RequiereModuloAttribute : TypeFilterAttribute
    {
        public RequiereModuloAttribute(string modulo) : base(typeof(RequiereModuloFilter))
        {
            Arguments = new object[] { modulo };
        }
    }

    public class RequiereModuloFilter : IAuthorizationFilter
    {
        private readonly string _modulo;
        private readonly PermisoService _permisoService;

        public RequiereModuloFilter(string modulo, PermisoService permisoService)
        {
            _modulo = modulo;
            _permisoService = permisoService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var rolClaim = context.HttpContext.User.FindFirst("Rol")?.Value;

            if (rolClaim == null || !Enum.TryParse<NombreRol>(rolClaim, out var rol))
            {
                context.Result = new ChallengeResult(); // no autenticado -> lo manda al login
                return;
            }

            if (!_permisoService.TienePermiso(rol, _modulo))
            {
                context.Result = new ForbidResult(); // autenticado pero sin permiso -> 403
            }
        }
    }
}
