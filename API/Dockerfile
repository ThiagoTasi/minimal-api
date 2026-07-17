FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo para dentro da pasta /app
COPY . .

# Restaura e publica de uma vez só
RUN dotnet publish -c Release -o /app/out

# Finalização
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Esta linha é essencial para o Render iniciar o serviço
ENTRYPOINT ["dotnet", "minimal_api.dll"]