ARG BASE_TAG=8.0
FROM mcr.microsoft.com/dotnet/sdk:${BASE_TAG}

# Establecer directorio de trabajo
WORKDIR /app

# Copiar y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el código fuente
COPY . ./

# Exponer el puerto
EXPOSE 8080

# Instalar dotnet-watch
RUN dotnet tool install -g dotnet-watch
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080"]
