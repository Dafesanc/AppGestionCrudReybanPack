# Makefile para gestión de Docker en el proyecto CRUD API

.PHONY: help build up down restart logs clean dev prod

# Ayuda por defecto
help:
	@echo "🐳 Comandos disponibles para CRUD API Docker:"
	@echo ""
	@echo "  make build     - Construir todas las imágenes"
	@echo "  make up        - Levantar entorno de producción"
	@echo "  make down      - Detener todos los servicios"
	@echo "  make restart   - Reiniciar todos los servicios"
	@echo "  make logs      - Ver logs de todos los servicios"
	@echo "  make clean     - Limpiar contenedores y volúmenes"
	@echo "  make dev       - Levantar entorno de desarrollo"
	@echo "  make prod      - Levantar entorno de producción"
	@echo "  make status    - Ver estado de servicios"
	@echo "  make api-logs  - Ver logs solo de la API"
	@echo "  make db-logs   - Ver logs solo de la base de datos"
	@echo ""

# Construir imágenes
build:
	@echo "📦 Construyendo imágenes..."
	docker-compose build --no-cache

# Levantar entorno de producción
up: build
	@echo "🚀 Levantando entorno de producción..."
	docker-compose up -d
	@echo "⏳ Esperando servicios..."
	@sleep 30
	@make status

# Levantar entorno de producción (alias)
prod: up

# Levantar entorno de desarrollo
dev:
	@echo "🔧 Levantando entorno de desarrollo..."
	docker-compose -f docker-compose.dev.yml up -d
	@echo "⏳ Esperando servicios..."
	@sleep 30
	@docker-compose -f docker-compose.dev.yml ps

# Detener servicios
down:
	@echo "🛑 Deteniendo servicios..."
	docker-compose down
	docker-compose -f docker-compose.dev.yml down 2>/dev/null || true

# Reiniciar servicios
restart:
	@echo "🔄 Reiniciando servicios..."
	docker-compose restart

# Ver logs de todos los servicios
logs:
	docker-compose logs -f

# Ver logs solo de la API
api-logs:
	docker-compose logs -f api

# Ver logs solo de la base de datos
db-logs:
	docker-compose logs -f sqlserver

# Ver estado de servicios
status:
	@echo "📊 Estado de servicios:"
	docker-compose ps

# Limpiar todo (⚠️ Elimina datos)
clean:
	@echo "🧹 Limpiando contenedores y volúmenes..."
	@echo "⚠️  Esto eliminará todos los datos almacenados"
	@read -p "¿Continuar? (y/N): " confirm && [ "$$confirm" = y ]
	docker-compose down -v
	docker-compose -f docker-compose.dev.yml down -v 2>/dev/null || true
	docker system prune -f

# Acceder al contenedor de la API
shell-api:
	docker-compose exec api bash

# Acceder al contenedor de base de datos
shell-db:
	docker-compose exec sqlserver bash

# Backup de base de datos
backup:
	@echo "💾 Creando backup de base de datos..."
	docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd \
		-S localhost -U sa -P YourStrongPassword123! \
		-Q "BACKUP DATABASE CrudApiDB TO DISK = '/tmp/backup.bak'"
	docker cp $$(docker-compose ps -q sqlserver):/tmp/backup.bak ./backup-$$(date +%Y%m%d_%H%M%S).bak
	@echo "✅ Backup creado: backup-$$(date +%Y%m%d_%H%M%S).bak"

# Test de endpoints
test:
	@echo "🧪 Probando endpoints de la API..."
	@echo "Health check:"
	@curl -s http://localhost:8080/health | head -1
	@echo ""
	@echo "Persons endpoint:"
	@curl -s http://localhost:8080/api/persons | head -1
	@echo ""

# Mostrar URLs útiles
urls:
	@echo "🌐 URLs disponibles:"
	@echo "  API Backend:  http://localhost:8080"
	@echo "  Swagger UI:   http://localhost:8080/swagger"
	@echo "  Health Check: http://localhost:8080/health"
	@echo "  Nginx Proxy:  http://localhost:80"
