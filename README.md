# API Atenciones Médicas

## Descripción
API REST desarrollada en .NET 8 con arquitectura limpia para la gestión de atenciones médicas. Incluye autenticación por API Key y documentación interactiva con Swagger.

## Características
- Arquitectura limpia.
- Autenticación mediante API Key.
- Swagger con soporte de autenticación.
- Configuración lista para despliegue.

## Requisitos previos
- .NET 8 SDK
- SQL Server
- Git

## Instalación y configuración
1. Clonar el repositorio:
   ```bash
   git clone <URL_REPOSITORIO>
   ```
2. Configurar `appsettings.json` con:
   - Cadena de conexión a la base de datos en `ConnectionStrings:DefaultConnection`.
   - Header y valor de API Key en `Security`.
3. Restaurar dependencias:
   ```bash
   dotnet restore
   ```
4. Ejecutar el proyecto:
   ```bash
   dotnet run --project API
   ```

## Uso
- Accede a Swagger en: `https://localhost:<puerto>/swagger`.
- Autentícate añadiendo tu API Key en el header configurado.
