# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Establece el directorio de trabajo en la imagen
WORKDIR /app

# Copia el proyecto actual en la imagen
COPY . .

RUN dotnet restore

# Publica el proyecto en la imagen
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Directorio de trabajo en la imagen
WORKDIR /app

# Copia el proyecto publicado en la imagen
COPY --from=build /app/out .

# Expone el puerto 8080
EXPOSE 8080

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Api.dll"]


