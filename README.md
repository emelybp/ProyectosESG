# Sistema de Control de Proyectos — Equipos y Sistemas de Generación

Proyecto integrador desarrollado para la materia Taller de Productividad
Basada en Herramientas Tecnológicas, Universidad TecMilenio.

Sistema web que centraliza el registro de proyectos, la documentación
asociada, los costos e ingresos, y el plan de trabajo de la empresa
Equipos y Sistemas de Generación, dedicada a la consultoría e ingeniería
de sistemas de generación de energía (plantas de emergencia,
motogeneradores, paneles solares, cogeneración, biodigestores y sistemas
de control/monitoreo).

## Tecnologías

- C# / ASP.NET Core MVC (.NET 8)
- Entity Framework Core + SQL Server Express
- xUnit para pruebas unitarias
- Bootstrap para la interfaz

## Estructura del repositorio

```
/src/SistemaControlProyectos      Aplicación web
/tests/SistemaControlProyectos.Tests   Pruebas unitarias (xUnit)
```

## Cómo ejecutarlo

1. Requiere .NET 8 SDK y SQL Server (o SQL Server Express) instalados.
2. Configurar la cadena de conexión en `appsettings.json` (no se incluye
   en el repositorio por seguridad; ver `.gitignore`).
3. Desde la carpeta `src/SistemaControlProyectos`:
   ```
   dotnet restore
   dotnet run
   ```
4. Para ejecutar las pruebas, desde la raíz del repositorio:
   ```
   dotnet test
   ```

## Documentación del proyecto

Este repositorio corresponde a la Fase 3 (Ejecución) del proyecto
integrador. La definición de requerimientos, el análisis de la
problemática y las decisiones de diseño están documentadas en la Fase 2
(entregada previamente).

## Nota sobre datos

Todos los datos de ejemplo usados en pruebas y capturas de pantalla son
ficticios. No se publican montos reales de costos, ingresos ni datos de
clientes de la empresa.
