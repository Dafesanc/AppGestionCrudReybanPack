# Contenerización con Docker

Esta aplicación está completamente contenerizada con Docker y lista para producción.

## Inicio Rápido con Docker

1. **Instalar Docker** (si no lo tienes): Ver [INSTALL-DOCKER.md](INSTALL-DOCKER.md)

2. **Levantar el entorno completo:**

   **Windows:**
   ```bash
   ./start-docker.bat
   ```

   **Linux/Mac:**
   ```bash
   chmod +x start-docker.sh
   ./start-docker.sh
   ```

3. **Acceder a los servicios:**
   - **API**: http://localhost:8080
   - **Swagger**: http://localhost:8080/swagger
   - **Nginx**: http://localhost:80

## Comandos Docker Útiles

```bash
# Ver servicios ejecutándose
docker-compose ps

# Ver logs de la API
docker-compose logs -f api

# Detener todo
docker-compose down

# Reiniciar servicios
docker-compose restart
```

## Entornos Disponibles

- **Producción**: `docker-compose up -d` (con Nginx + SSL)
- **Desarrollo**: `docker-compose -f docker-compose.dev.yml up -d`

Ver documentación completa: [DOCKER-README.md](DOCKER-README.md)

# Arquitectura de Contenedores

```
┌─────────────┐   ┌─────────────┐   ┌─────────────┐
│    Nginx    │   │  API .NET   │   │ SQL Server  │
│   (Proxy)   │──▶│    Core     │──▶│    2022     │
│  Port: 80   │   │ Port: 8080  │   │ Port: 1433  │
└─────────────┘   └─────────────┘   └─────────────┘
```