# =============== common ===============
ARG BASE_TAG=8.0

# =============== build stage ===============
FROM mcr.microsoft.com/dotnet/sdk:${BASE_TAG} AS build
WORKDIR /src
COPY ./MRCD.sln ./
COPY ./MRCD.API/MRCD.API.csproj ./MRCD.API/
COPY ./MRCD.Domain/MRCD.Domain.csproj ./MRCD.Domain/
COPY ./MRCD.Application/MRCD.Application.csproj ./MRCD.Application/
COPY ./MRCD.Infrastructure/MRCD.Infrastructure.csproj ./MRCD.Infrastructure/
#COPY ./MRCD.Test/MRCD.Test.csproj ./MRCD.Test/
RUN dotnet restore ./MRCD.sln
COPY ./ ./
RUN dotnet publish ./MRCD.API/MRCD.API.csproj -c Release -o /out/api /p:UseAppHost=false

# =============== api runtime =================
FROM mcr.microsoft.com/dotnet/aspnet:${BASE_TAG} AS api
WORKDIR /app
COPY --from=build /out/api ./
RUN apt-get update && apt-get upgrade -y && apt-get autoremove --purge -y
RUN adduser --disabled-password appuser && chown -R appuser:appuser /app
USER appuser
EXPOSE 8080
ENTRYPOINT ["dotnet", "MRCD.API.dll"]
