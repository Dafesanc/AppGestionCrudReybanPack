@echo off
REM Script para Windows - Levantar el entorno completo con Docker Compose

echo 🐳 Iniciando entorno CRUD API con Docker...

REM Verificar si Docker está ejecutándose
docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ Error: Docker no está ejecutándose. Por favor inicia Docker Desktop.
    pause
    exit /b 1
)

REM Verificar si Docker Compose está disponible
docker-compose --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ Error: Docker Compose no está instalado.
    pause
    exit /b 1
)

REM Construir y levantar los servicios
echo 📦 Construyendo imágenes...
docker-compose build --no-cache

echo 🚀 Levantando servicios...
docker-compose up -d

REM Esperar a que los servicios estén listos
echo ⏳ Esperando a que los servicios estén listos...
timeout /t 30 /nobreak >nul

REM Verificar el estado de los servicios
echo 📊 Estado de los servicios:
docker-compose ps

REM Mostrar logs de la API para verificar que esté funcionando
echo 📝 Logs de la API (últimas 20 líneas):
docker-compose logs --tail=20 api

echo.
echo ✅ Entorno levantado exitosamente!
echo.
echo 🌐 Servicios disponibles:
echo    - API Backend: http://localhost:8080
echo    - Swagger UI: http://localhost:8080/swagger
echo    - Nginx Proxy: http://localhost:80
echo    - SQL Server: localhost:1433
echo.
echo 🔧 Comandos útiles:
echo    - Ver logs: docker-compose logs -f [servicio]
echo    - Detener: docker-compose down
echo    - Reiniciar: docker-compose restart [servicio]
echo    - Ver estado: docker-compose ps
echo.
pause
