#!/bin/bash

# Script para levantar el entorno completo con Docker Compose

echo "🐳 Iniciando entorno CRUD API con Docker..."

# Verificar si Docker está ejecutándose
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker no está ejecutándose. Por favor inicia Docker Desktop."
    exit 1
fi

# Verificar si Docker Compose está disponible
if ! command -v docker-compose > /dev/null 2>&1; then
    echo "❌ Error: Docker Compose no está instalado."
    exit 1
fi

# Construir y levantar los servicios
echo "📦 Construyendo imágenes..."
docker-compose build --no-cache

echo "🚀 Levantando servicios..."
docker-compose up -d

# Esperar a que los servicios estén listos
echo "⏳ Esperando a que los servicios estén listos..."
sleep 30

# Verificar el estado de los servicios
echo "📊 Estado de los servicios:"
docker-compose ps

# Mostrar logs de la API para verificar que esté funcionando
echo "📝 Logs de la API (últimas 20 líneas):"
docker-compose logs --tail=20 api

echo ""
echo "✅ Entorno levantado exitosamente!"
echo ""
echo "🌐 Servicios disponibles:"
echo "   - API Backend: http://localhost:8080"
echo "   - Swagger UI: http://localhost:8080/swagger"
echo "   - Nginx Proxy: http://localhost:80"
echo "   - SQL Server: localhost:1433"
echo ""
echo "🔧 Comandos útiles:"
echo "   - Ver logs: docker-compose logs -f [servicio]"
echo "   - Detener: docker-compose down"
echo "   - Reiniciar: docker-compose restart [servicio]"
echo "   - Ver estado: docker-compose ps"
