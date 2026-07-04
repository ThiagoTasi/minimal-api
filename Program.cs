using Microsoft.EntityFrameworkCore;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.DTOs;

var builder = WebApplication.CreateBuilder(args);

// A CONFIGURAÇÃO DO BANCO TEM QUE FICAR AQUI (ANTES DO BUILD!)
builder.Services.AddDbContext<DbContexto>(options =>
{
    var stringConexao = builder.Configuration.GetConnectionString("mysql");
    options.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
});

// O BUILD VEM DEPOIS QUE TUDO FOI CONFIGURADO
var app = builder.Build();

app.MapGet("/", () => "Olá pessoal!");

app.MapPost("/login", (LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456")
    {
        return Results.Ok("Login com sucesso!");
    }
    else
    {
        return Results.Unauthorized();
    }
});

app.Run();


