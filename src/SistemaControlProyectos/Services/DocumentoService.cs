using SistemaControlProyectos.Models;

namespace SistemaControlProyectos.Services
{
    // Lógica de negocio para la carga de documentos (RF-03, Fase 2 sección 5.1)
    public class DocumentoService
    {
        public static readonly string[] ExtensionesPermitidas =
            { ".pdf", ".jpg", ".jpeg", ".png", ".docx", ".xlsx" };

        public const long TamanioMaximoBytes = 10 * 1024 * 1024; // 10 MB

        // Validación completa a partir del ViewModel (usada por el controlador)
        public List<string> Validar(DocumentoViewModel vm)
        {
            var errores = new List<string>();

            if (vm.ProyectoId <= 0)
                errores.Add("El documento debe estar asociado a un proyecto.");

            if (vm.Archivo == null || vm.Archivo.Length == 0)
                errores.Add("Debe seleccionar un archivo.");
            else
                errores.AddRange(ValidarArchivo(vm.Archivo.FileName, vm.Archivo.Length));

            return errores;
        }

        // Validación pura del archivo (nombre y tamaño), fácil de probar sin IFormFile real
        public List<string> ValidarArchivo(string? nombreArchivo, long tamanioBytes)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(nombreArchivo))
            {
                errores.Add("Debe seleccionar un archivo.");
                return errores;
            }

            var extension = Path.GetExtension(nombreArchivo).ToLowerInvariant();
            if (!ExtensionesPermitidas.Contains(extension))
                errores.Add($"Tipo de archivo no permitido ({extension}). Extensiones válidas: {string.Join(", ", ExtensionesPermitidas)}.");

            if (tamanioBytes > TamanioMaximoBytes)
                errores.Add("El archivo no debe superar los 10 MB.");

            return errores;
        }

        // Genera un nombre de archivo único para evitar sobrescribir documentos con el mismo nombre
        public string GenerarNombreUnico(string nombreOriginal)
        {
            var nombreLimpio = Path.GetFileName(nombreOriginal);
            return $"{Guid.NewGuid():N}_{nombreLimpio}";
        }

        public Documento CrearDocumento(DocumentoViewModel vm, string rutaRelativa)
        {
            return new Documento
            {
                ProyectoId = vm.ProyectoId,
                Tipo = vm.Tipo,
                RutaArchivo = rutaRelativa,
                FechaCarga = DateTime.Now,
                FechaVigencia = vm.FechaVigencia
            };
        }
    }
}
