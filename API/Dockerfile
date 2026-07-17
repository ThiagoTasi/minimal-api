FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia tudo para dentro da pasta /app
COPY . .

# Restaura e publica de uma vez só
RUN dotnet publish -c Release -o /app/out

# Finalização - Usando a versão 9.0 do ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "MinimalApi.dll"]