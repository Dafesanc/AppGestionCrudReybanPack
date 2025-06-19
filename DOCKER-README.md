# 🐳 Documentación de Contenerización Docker

Esta documentación explica cómo usar Docker para levantar la API CRUD de Personas y Mascotas en contenedores.

## 📋 Prerrequisitos

- **Docker Desktop** instalado y ejecutándose
- **Docker Compose** (incluido con Docker Desktop)
- Al menos **4GB de RAM** disponible para los contenedores
- Puertos **80**, **1433**, **8080** disponibles

## 🏗️ Arquitectura de Contenedores

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│     Nginx       │    │   API Backend   │    │   SQL Server    │
│   (Proxy)       │    │   (.NET 8)      │    │    (2022)       │
│   Port: 80      │───▶│   Port: 8080    │───▶│   Port: 1433    │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 🚀 Inicio Rápido

### Opción 1: Script Automatizado (Recomendado)

**Windows:**
```bash
./start-docker.bat
```

**Linux/Mac:**
```bash
chmod +x start-docker.sh
./start-docker.sh
```

### Opción 2: Comandos Manuales

```bash
# Construir las imágenes
docker-compose build

# Levantar todos los servicios
docker-compose up -d

# Ver el estado
docker-compose ps

# Ver logs
docker-compose logs -f api
```

## 🌐 Servicios Disponibles

Una vez levantado el entorno, tendrás acceso a:

| Servicio | URL | Descripción |
|----------|-----|-------------|
| **API Backend** | http://localhost:8080 | API REST principal |
| **Swagger UI** | http://localhost:8080/swagger | Documentación interactiva |
| **Nginx Proxy** | http://localhost:80 | Proxy reverso |
| **SQL Server** | localhost:1433 | Base de datos |
| **Health Check** | http://localhost:8080/health | Estado de la API |

### Credenciales de Base de Datos

- **Server:** localhost,1433
- **Usuario:** sa
- **Contraseña:** YourStrongPassword123!
- **Database:** CrudApiDB

## 📁 Estructura de Archivos Docker

```
📦 Proyecto/
├── 🐳 Dockerfile                 # Imagen de la API
├── 🐳 docker-compose.yml         # Entorno de producción
├── 🐳 docker-compose.dev.yml     # Entorno de desarrollo
├── 🐳 .dockerignore              # Archivos a ignorar
├── 📁 nginx/
│   └── nginx.conf                # Configuración del proxy
├── 📁 init-scripts/
│   └── 01-init-database.sql      # Script de inicialización de BD
├── 🚀 start-docker.bat           # Script de inicio (Windows)
└── 🚀 start-docker.sh            # Script de inicio (Linux/Mac)
```

## 🔧 Comandos Útiles

### Gestión de Servicios

```bash
# Levantar servicios
docker-compose up -d

# Detener servicios
docker-compose down

# Reiniciar un servicio específico
docker-compose restart api

# Ver estado de servicios
docker-compose ps

# Ver logs en tiempo real
docker-compose logs -f api

# Ver logs de todos los servicios
docker-compose logs -f
```

### Debugging y Mantenimiento

```bash
# Acceder al contenedor de la API
docker-compose exec api bash

# Acceder al contenedor de SQL Server
docker-compose exec sqlserver bash

# Ver uso de recursos
docker stats

# Limpiar volúmenes (⚠️ Elimina datos)
docker-compose down -v

# Reconstruir imágenes
docker-compose build --no-cache
```

## 🏭 Entornos

### Producción (docker-compose.yml)

- **Nginx** como proxy reverso
- **SQL Server** con datos persistentes
- **API** optimizada para producción
- **Health checks** configurados
- **Logs** estructurados

```bash
docker-compose up -d
```

### Desarrollo (docker-compose.dev.yml)

- **Sin Nginx** (acceso directo)
- **Hot reload** habilitado
- **Puerto diferente** (8081)
- **Configuración de desarrollo**

```bash
docker-compose -f docker-compose.dev.yml up -d
```

## 🔒 Configuración de Seguridad

### Variables de Entorno Sensibles

Para producción, considera usar un archivo `.env`:

```env
# .env
SA_PASSWORD=TuPasswordSegura123!
API_ENVIRONMENT=Production
```

### Redes Docker

Los contenedores se comunican a través de una red privada:
- **Red:** crud-api-network
- **Aislamiento** de otros contenedores
- **Comunicación interna** por nombre de servicio

## 📊 Monitoreo y Logs

### Health Checks

Cada servicio tiene health checks configurados:

```bash
# Ver estado de salud
docker-compose ps

# Estado detallado
docker inspect $(docker-compose ps -q api) | grep Health -A 10
```

### Logs Estructurados

```bash
# Logs de la API con timestamps
docker-compose logs -f --timestamps api

# Logs de SQL Server
docker-compose logs -f sqlserver

# Logs de Nginx
docker-compose logs -f nginx
```

## 🗄️ Persistencia de Datos

### Volúmenes Docker

Los datos se almacenan en volúmenes persistentes:

- **sqlserver_data:** Datos de SQL Server
- **Ubicación:** Docker volumes

```bash
# Ver volúmenes
docker volume ls

# Inspeccionar volumen
docker volume inspect crud-api-sqlserver-data
```

### Backup y Restore

```bash
# Backup de la base de datos
docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P YourStrongPassword123! \
  -Q "BACKUP DATABASE CrudApiDB TO DISK = '/tmp/backup.bak'"

# Copiar backup al host
docker cp $(docker-compose ps -q sqlserver):/tmp/backup.bak ./backup.bak
```

## 🚨 Solución de Problemas

### Problemas Comunes

#### 1. Puerto ya en uso
```bash
# Verificar qué está usando el puerto
netstat -ano | findstr :8080

# Cambiar puerto en docker-compose.yml
ports:
  - "8081:8080"  # Cambiar puerto host
```

#### 2. SQL Server no inicia
```bash
# Ver logs detallados
docker-compose logs sqlserver

# Verificar recursos disponibles
docker stats
```

#### 3. API no se conecta a la base de datos
```bash
# Verificar red
docker network ls
docker network inspect crud-api-network

# Probar conectividad
docker-compose exec api ping sqlserver
```

### Logs de Debug

```bash
# Habilitar logs verbosos
docker-compose -f docker-compose.yml -f docker-compose.debug.yml up -d
```

## 📈 Escalabilidad

### Múltiples Instancias de API

```yaml
# En docker-compose.yml
api:
  # ... configuración base
  deploy:
    replicas: 3
```

### Load Balancer

Nginx está configurado para balancear carga entre múltiples instancias de la API.

## 🔄 CI/CD Integration

### GitHub Actions Example

```yaml
name: Docker Build and Deploy

on:
  push:
    branches: [ main ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v2
    
    - name: Build and Deploy
      run: |
        docker-compose build
        docker-compose up -d
```

## 📝 Notas Importantes

1. **Primer inicio:** Puede tardar varios minutos en inicializar SQL Server
2. **Datos de ejemplo:** Se insertan automáticamente al crear la base de datos
3. **Persistencia:** Los datos sobreviven a reinicios de contenedores
4. **Seguridad:** Cambiar passwords por defecto en producción
5. **Recursos:** Monitorear uso de CPU y memoria

## 🆘 Soporte

Si encuentras problemas:

1. **Verificar logs:** `docker-compose logs -f`
2. **Revisar estado:** `docker-compose ps`
3. **Limpiar y reiniciar:** `docker-compose down && docker-compose up -d`
4. **Reinicio completo:** `docker-compose down -v && docker-compose up -d`
