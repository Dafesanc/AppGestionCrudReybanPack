# 🛠️ Guía de Instalación de Docker

## Para Windows

### Opción 1: Docker Desktop (Recomendada)

1. **Descargar Docker Desktop:**
   - Ve a https://www.docker.com/products/docker-desktop/
   - Descarga "Docker Desktop for Windows"

2. **Instalar:**
   - Ejecuta el instalador descargado
   - Sigue las instrucciones del instalador
   - Reinicia el sistema si es necesario

3. **Verificar instalación:**
   ```powershell
   docker --version
   docker-compose --version
   ```

### Opción 2: Docker con WSL2

1. **Habilitar WSL2:**
   ```powershell
   wsl --install
   ```

2. **Instalar Docker Desktop con WSL2 backend**

## Para Linux (Ubuntu/Debian)

```bash
# Actualizar paquetes
sudo apt update

# Instalar dependencias
sudo apt install apt-transport-https ca-certificates curl software-properties-common

# Agregar clave GPG de Docker
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -

# Agregar repositorio
sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"

# Instalar Docker
sudo apt update
sudo apt install docker-ce

# Instalar Docker Compose
sudo curl -L "https://github.com/docker/compose/releases/download/v2.20.0/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Agregar usuario al grupo docker
sudo usermod -aG docker $USER
```

## Para macOS

### Opción 1: Docker Desktop
1. Descargar desde https://www.docker.com/products/docker-desktop/
2. Instalar el archivo .dmg
3. Ejecutar Docker Desktop

### Opción 2: Homebrew
```bash
brew install --cask docker
```

## Verificación Post-Instalación

Después de instalar Docker, verifica que funcione correctamente:

```bash
# Verificar versión
docker --version
docker-compose --version

# Probar con una imagen simple
docker run hello-world

# Verificar que Docker esté ejecutándose
docker info
```

## Configuración Inicial

### Recursos Recomendados para Docker Desktop

- **CPU:** Mínimo 2 núcleos, recomendado 4+
- **RAM:** Mínimo 4GB, recomendado 8GB+
- **Disco:** Al menos 20GB de espacio libre

### Configurar Docker Desktop

1. Abrir Docker Desktop
2. Ir a Settings/Preferences
3. Ajustar recursos:
   - **Memory:** 4GB mínimo para la aplicación
   - **CPU:** 2+ núcleos
   - **Swap:** 1GB

## Comandos Básicos Post-Instalación

```bash
# Ver imágenes descargadas
docker images

# Ver contenedores ejecutándose
docker ps

# Ver todos los contenedores
docker ps -a

# Limpiar sistema (opcional)
docker system prune
```

## Solución de Problemas Comunes

### Windows: "Docker Desktop requires Windows 10 Pro or Enterprise"

**Solución:** Usar Docker Toolbox o actualizar a Windows 10 Pro/Enterprise

### Linux: "Permission denied"

**Solución:**
```bash
sudo usermod -aG docker $USER
# Cerrar sesión y volver a iniciar
```

### macOS: "Docker Desktop failed to start"

**Solución:**
```bash
# Reiniciar Docker Desktop
killall Docker && open /Applications/Docker.app
```

## Una vez instalado Docker

Regresa a la carpeta del proyecto y ejecuta:

```bash
# Windows
./start-docker.bat

# Linux/Mac
chmod +x start-docker.sh
./start-docker.sh
```

## Recursos Adicionales

- **Documentación oficial:** https://docs.docker.com/
- **Docker Hub:** https://hub.docker.com/
- **Tutoriales:** https://docs.docker.com/get-started/
