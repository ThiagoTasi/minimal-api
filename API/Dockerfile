# Use a imagem oficial do SDK do .NET 8.0 para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos da solução
COPY *.sln ./

# Copia os projetos para suas respectivas pastas
COPY API/*.csproj ./API/
COPY Test/*.csproj ./Test/

# Restaura as dependências de toda a solução
RUN dotnet restore

# Copia todo o código fonte
COPY . .

# Publica a API (ajustado para o seu caminho)
RUN dotnet publish API/minimal_api.csproj -c Release -o out

# Usa a imagem de runtime do .NET 8.0 para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Define a porta que o Render vai usar (8080 é o padrão do Render para containers)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "minimal_api.dll"]