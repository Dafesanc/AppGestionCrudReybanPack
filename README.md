# API CRUD - Gestión de Personas y Mascotas

## Descripción

Esta API REST desarrollada en .NET 8 con C# proporciona operaciones CRUD completas para la gestión de personas y mascotas, siguiendo las mejores prácticas de desarrollo y arquitectura MVC.

## Características

- ✅ Arquitectura MVC bien estructurada
- ✅ Entity Framework Core para acceso a datos
- ✅ AutoMapper para mapeo de DTOs
- ✅ Validación de datos completa
- ✅ Respuestas API estandarizadas
- ✅ Documentación Swagger
- ✅ Logging estructurado
- ✅ Manejo de errores centralizado
- ✅ CORS configurado

## Estructura del Proyecto

```
BackEndApi/
├── Controllers/          # Controladores MVC
│   ├── PersonsController.cs
│   └── PetsController.cs
├── Models/              # Entidades de dominio
│   ├── Person.cs
│   └── Pet.cs
├── DTOs/                # Data Transfer Objects
│   ├── PersonDtos.cs
│   └── PetDtos.cs
├── Data/                # Contexto de base de datos
│   └── ApplicationDbContext.cs
├── Interfaces/          # Contratos/Interfaces
│   ├── IGenericRepository.cs
│   ├── IPersonRepository.cs
│   └── IPetRepository.cs
├── Repositories/        # Implementación de repositorios
│   ├── GenericRepository.cs
│   ├── PersonRepository.cs
│   └── PetRepository.cs
├── Mappings/           # Configuración AutoMapper
│   └── AutoMapperProfile.cs
└── Common/             # Clases comunes
    └── ApiResponse.cs
```

## Tecnologías Utilizadas

- .NET 8
- C# 12
- Entity Framework Core 8.0
- AutoMapper 12.0
- SQL Server
- Swagger/OpenAPI

## Configuración

### 1. Cadena de Conexión

Actualiza la cadena de conexión en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=TU_BASE_DATOS;Trusted_Connection=true;"
  }
}
```

### 2. Base de Datos

Las tablas deben existir en tu base de datos:

```sql
CREATE TABLE Persons (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) UNIQUE NOT NULL,
    BirthDate DATE,
    Height DECIMAL(5,2),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Pets (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Species NVARCHAR(50) NOT NULL,
    Breed NVARCHAR(100),
    Color NVARCHAR(50),
    Age INT,
    CreatedAt DATETIME DEFAULT GETDATE()
);
```

## Endpoints Disponibles

### Personas (Persons)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/persons` | Obtener todas las personas |
| GET | `/api/persons/{id}` | Obtener persona por ID |
| POST | `/api/persons` | Crear nueva persona |
| PUT | `/api/persons/{id}` | Actualizar persona |
| DELETE | `/api/persons/{id}` | Eliminar persona |
| GET | `/api/persons/search?searchTerm={term}` | Buscar personas por nombre |

### Mascotas (Pets)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/pets` | Obtener todas las mascotas |
| GET | `/api/pets/{id}` | Obtener mascota por ID |
| POST | `/api/pets` | Crear nueva mascota |
| PUT | `/api/pets/{id}` | Actualizar mascota |
| DELETE | `/api/pets/{id}` | Eliminar mascota |
| GET | `/api/pets/search?searchTerm={term}` | Buscar mascotas por nombre |
| GET | `/api/pets/species/{species}` | Obtener mascotas por especie |
| GET | `/api/pets/age-range?minAge={min}&maxAge={max}` | Obtener mascotas por rango de edad |

## Ejemplos de Uso

### Crear una Persona

```http
POST /api/persons
Content-Type: application/json

{
  "firstName": "Juan",
  "lastName": "Pérez",
  "email": "juan.perez@email.com",
  "birthDate": "1990-05-15",
  "height": 175.5
}
```

### Crear una Mascota

```http
POST /api/pets
Content-Type: application/json

{
  "name": "Max",
  "species": "Perro",
  "breed": "Golden Retriever",
  "color": "Dorado",
  "age": 3
}
```

## Respuestas de la API

### Respuesta Exitosa

```json
{
  "success": true,
  "message": "Operación exitosa",
  "data": { /* objeto de respuesta */ },
  "errors": [],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Respuesta de Error

```json
{
  "success": false,
  "message": "Descripción del error",
  "data": null,
  "errors": ["Error específico 1", "Error específico 2"],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

## Códigos de Estado HTTP

- `200 OK` - Operación exitosa
- `201 Created` - Recurso creado exitosamente
- `400 Bad Request` - Datos de entrada inválidos
- `404 Not Found` - Recurso no encontrado
- `409 Conflict` - Conflicto (ej: email duplicado)
- `500 Internal Server Error` - Error interno del servidor

## Ejecución

1. Restaurar paquetes:
```bash
dotnet restore
```

2. Ejecutar la aplicación:
```bash
dotnet run
```

3. Acceder a Swagger: `https://localhost:7xxx` (el puerto puede variar)

## Validaciones Implementadas

### Personas
- FirstName: Requerido, máximo 100 caracteres
- LastName: Requerido, máximo 100 caracteres
- Email: Requerido, formato válido, único, máximo 150 caracteres
- Height: Entre 0 y 300 cm
- BirthDate: Fecha válida

### Mascotas
- Name: Requerido, máximo 100 caracteres
- Species: Requerido, máximo 50 caracteres
- Breed: Opcional, máximo 100 caracteres
- Color: Opcional, máximo 50 caracteres
- Age: Entre 0 y 50 años

## Mejores Prácticas Implementadas

1. **Separación de Responsabilidades**: Uso del patrón Repository
2. **DTOs**: Transferencia de datos segura sin exponer entidades
3. **AutoMapper**: Mapeo automático entre entidades y DTOs
4. **Validación**: Validaciones tanto en DTOs como en entidades
5. **Manejo de Errores**: Respuestas consistentes para todos los casos
6. **Logging**: Registro de eventos importantes y errores
7. **Documentación**: Swagger/OpenAPI automático
8. **CORS**: Configurado para desarrollo
9. **Async/Await**: Operaciones asíncronas para mejor rendimiento
