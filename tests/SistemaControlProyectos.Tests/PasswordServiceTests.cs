using SistemaControlProyectos.Models;
using SistemaControlProyectos.Services;
using Xunit;

namespace SistemaControlProyectos.Tests
{
    public class PasswordServiceTests
    {
        [Fact]
        public void HashPassword_YVerificarConContrasenaCorrecta_DevuelveTrue()
        {
            var servicio = new PasswordService();
            var usuario = new Usuario { Correo = "ana@esgsa.com.mx" };

            var hash = servicio.HashPassword(usuario, "MiContrasena123!");

            Assert.True(servicio.VerificarPassword(usuario, hash, "MiContrasena123!"));
        }

        [Fact]
        public void VerificarPassword_ConContrasenaIncorrecta_DevuelveFalse()
        {
            var servicio = new PasswordService();
            var usuario = new Usuario { Correo = "ana@esgsa.com.mx" };
            var hash = servicio.HashPassword(usuario, "MiContrasena123!");

            var esValida = servicio.VerificarPassword(usuario, hash, "otra-contrasena");

            Assert.False(esValida);
        }

        [Fact]
        public void HashPassword_NuncaDevuelveElTextoPlano()
        {
            var servicio = new PasswordService();
            var usuario = new Usuario { Correo = "ana@esgsa.com.mx" };

            var hash = servicio.HashPassword(usuario, "secreta123");

            Assert.DoesNotContain("secreta123", hash);
        }
    }
}
