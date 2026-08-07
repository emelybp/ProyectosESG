using Microsoft.AspNetCore.Identity;
using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Services
{
    // Protección de contraseñas: nunca se guarda ni se compara texto plano (Fase 2 sección 5.6.1)
    public class PasswordService
    {
        private readonly PasswordHasher<Usuario> _hasher = new();

        public string HashPassword(Usuario usuario, string contrasenaPlano)
        {
            return _hasher.HashPassword(usuario, contrasenaPlano);
        }

        public bool VerificarPassword(Usuario usuario, string hashGuardado, string contrasenaIngresada)
        {
            var resultado = _hasher.VerifyHashedPassword(usuario, hashGuardado, contrasenaIngresada);
            return resultado == PasswordVerificationResult.Success
                || resultado == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
