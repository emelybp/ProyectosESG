# Guía de contribución

Gracias por interesarte en el **Sistema de Control de Proyectos – ESG**. Este documento
explica cómo trabajar sobre el repositorio sin romper el flujo de trabajo del proyecto.

## Antes de empezar

1. Revisa la lista de [issues abiertos](https://github.com/emelybp/ProyectosESG/issues).
2. Elige uno que no esté asignado y coméntalo para que quede claro que lo estás tomando.
3. Si vas a proponer algo nuevo, abre primero un issue describiendo **qué** quieres hacer y
   **por qué**, antes de escribir código.

Todos los issues de este repositorio siguen la misma estructura interna:

- **Descripción:** qué debe hacer el sistema.
- **Análisis:** por qué es necesario y qué implica.
- **Solución:** cómo se va a resolver técnicamente.

## Ramas

| Rama | Para qué sirve |
|---|---|
| `master` | Versión liberada y estable. No se trabaja directamente sobre ella. |
| `develop` | Rama de integración. De aquí sale y aquí regresa todo el trabajo. |
| `feature/RF-XX-descripcion` | Una rama por tarea o requerimiento. |

## Flujo de trabajo paso a paso

### 1. Clonar el repositorio

```bash
git clone https://github.com/emelybp/ProyectosESG.git
cd ProyectosESG
```

### 2. Colocarte en `develop` y actualizarla

```bash
git checkout develop
git pull origin develop
```

### 3. Crear tu rama de tarea

El nombre debe llevar el número de requerimiento y una descripción corta en minúsculas y
separada por guiones:

```bash
git checkout -b feature/RF-05-registrar-costo
```

### 4. Trabajar y confirmar los cambios

Haz commits pequeños y con mensajes que empiecen con el número de requerimiento:

```bash
git add .
git commit -m "RF-05: validación del monto en el registro de costos"
```

Antes de subir, verifica que todo compile y que las pruebas pasen:

```bash
dotnet build
dotnet test
```

### 5. Subir la rama

```bash
git push origin feature/RF-05-registrar-costo
```

### 6. Abrir el pull request

- Base: `develop` · Compare: tu rama `feature/…`
- Título: el número y nombre del requerimiento (por ejemplo, `RF-05 Registrar costo`).
- En la descripción, escribe `Closes #NN` con el número del issue, para que se cierre solo
  al integrarse.

### 7. Esperar la revisión

El workflow de integración continua (GitHub Actions) se ejecuta automáticamente y corre la
compilación y las pruebas con xUnit. **Un pull request con la CI en rojo no se integra.**

### 8. Merge

Una vez aprobado y con la CI en verde, se hace el merge hacia `develop`. Al integrarse:

- El issue se cierra automáticamente.
- La tarjeta correspondiente se mueve sola a la columna **Done** del tablero de Zube.
- La rama de tarea se puede eliminar.

## Estilo de código

- Nombres de clases y métodos en **PascalCase**; variables locales en **camelCase**.
- Nombres en español para las entidades del negocio (`Proyecto`, `Documento`, `Costo`),
  igual que en el modelo de datos.
- Toda la lógica de negocio vive en la capa de servicios, no en los controladores.
- Cada corrección o funcionalidad nueva debe venir acompañada de su prueba unitaria.

## Qué no se sube al repositorio

- Cadenas de conexión, contraseñas o credenciales reales.
- Carpetas `bin/` y `obj/`.
- Archivos de documentos de prueba con información real de clientes.

## Contacto

Dudas sobre el flujo de trabajo: abre un issue con la etiqueta `pregunta` o escribe a
[@emelybp](https://github.com/emelybp).
