# 🏥 API de Gestión de Atenciones Médicas

API REST desarrollada en **.NET 8** con **Dapper** y **SQL Server** para la gestión de pacientes, doctores, especialidades y atenciones médicas.  
Incluye seguridad por **API Key**, documentación con **Swagger**, validaciones y scripts SQL listos para ejecutar.

---

## 📋 Requisitos

- .NET 8 SDK
- SQL Server Express/Developer
- Postman

---

## 🚀 Instalación y Configuración

1️⃣ **Clonar el repositorio**
```bash
git clone <URL_DEL_REPOSITORIO>
cd <NOMBRE_DEL_REPO>
```

2️⃣ Restaurar la base de datos
En la carpeta Scripts/ se incluyen:

- schema_full.sql → Crea las tablas con constraints e índices y Procedimientos almacenados.

- seed.sql → Inserta datos iniciales para pruebas.

Ejecuta en SQL Server Management Studio (SSMS) en este orden:

- schema_full.sql

- seed.sql

---

## 🔑 Seguridad API Key
- Todas las peticiones deben incluir el header configurado en appsettings.json:

	- Nombre: X-API-Key

	- Valor: el definido en "ApiKeys" (super-secret-key o el que cambies).

---

## 📚 Documentación Swagger
- Con la API en ejecución, abre: http://localhost:5051/swagger

---

## 📌 Endpoints Principales
### Pacientes
- GET /api/pacientes → Listar todos

- GET /api/pacientes/{id} → Obtener por ID

- GET /api/pacientes/search?name=Juan&rut=12345678-9 → Búsqueda filtrada

- POST /api/pacientes → Crear

- PUT /api/pacientes/{id} → Actualizar

- DELETE /api/pacientes/{id} → Eliminar

### Doctores
- CRUD completo similar a pacientes.

- Búsqueda por nombre y especialidad.

### Especialidades
- CRUD completo.

- No se pueden eliminar si tienen doctores asociados (HTTP 409).

### Atenciones
- GET /api/atenciones?fromUtc=YYYY-MM-DDTHH:mm:ssZ&toUtc=YYYY-MM-DDTHH:mm:ssZ&status=Completed

- POST /api/atenciones → Crear (valida rango de fechas y disponibilidad)

- PUT /api/atenciones/{id} → Actualizar

- DELETE /api/atenciones/{id} → Eliminar

---

## 🧪 Pruebas
Se incluye en /Test:

Archivo pruebas_postman.txt con todas las peticiones.

---

## 📂 Estructura del proyecto

```bash
/API                  # Proyecto principal
/Application          # Casos de uso
/Domain               # Entidades y contratos
/Persistence          # Repositorios e infraestructura de datos
/Scripts              # Scripts SQL
/Tests                # Colección Postman
appsettings.example.json
README.md
```

---

## 👨‍💻 Autor
Desarrollado por Oscar Aguilera como prueba técnica.
