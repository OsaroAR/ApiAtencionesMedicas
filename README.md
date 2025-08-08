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
- SQL Server, SQL Server Management Studio (SSMS) para importar el backup
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
   
## Importar backup BACPAC
Para restaurar la base de datos MedicalCare desde el archivo .bacpac incluido en el repositorio:

1. Abrir SQL Server Management Studio (SSMS).
2. Conectarse a la instancia de SQL Server.
3. En el Explorador de Objetos, clic derecho en Bases de datos → Import Data-tier Application....
4. Seleccionar Import from local disk y elegir el archivo .bacpac incluido en la carpeta del proyecto.
5. Definir el nombre de la base de datos (por ejemplo, MedicalCare) y la ruta de almacenamiento de archivos.
6. Completar el asistente y verificar que la base de datos se haya creado correctamente.

## Uso
- Accede a Swagger en: `https://localhost:<puerto>/swagger`.
- Autentícate añadiendo tu API Key en el header configurado.
