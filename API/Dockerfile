# Usa a imagem oficial do SDK do .NET 8.0 para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia apenas o arquivo do projeto
COPY *.csproj ./

# Restaura as dependências
RUN dotnet restore

# Copia todo o código fonte da pasta atual (API)
COPY . .

# Publica a API
RUN dotnet publish -c Release -o out

# Usa a imagem de runtime do .NET 8.0 para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Define a porta que o Render vai usar
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "minimal_api.dll"]