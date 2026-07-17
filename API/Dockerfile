FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia tudo para a pasta /app
COPY . .

# Compila dentro da pasta API
RUN dotnet publish "API/minimal_api.csproj" -c Release -o /app/out

# Finalização
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "MinimalApi.dll"]