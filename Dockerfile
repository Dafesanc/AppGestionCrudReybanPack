# Dockerfile para la API Backend
# Usar la imagen oficial de .NET SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de proyecto y restaurar dependencias
COPY BackEndApi/*.csproj ./BackEndApi/
RUN dotnet restore BackEndApi/BackEndApi.csproj

# Copiar todo el código fuente
COPY BackEndApi/ ./BackEndApi/

# Compilar la aplicación
WORKDIR /app/BackEndApi
RUN dotnet publish -c Release -o out

# Usar la imagen de runtime para la ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Instalar cliente de SQL Server para herramientas de diagnóstico
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copiar la aplicación compilada
COPY --from=build /app/BackEndApi/out .

# Crear un usuario no-root para seguridad
RUN groupadd -r appuser && useradd -r -g appuser appuser
RUN chown -R appuser:appuser /app
USER appuser

# Exponer el puerto
EXPOSE 8080

# Variables de entorno
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Comando de inicio
ENTRYPOINT ["dotnet", "BackEndApi.dll"]
