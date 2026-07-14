// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Mvc;
// using MinimalApi.Infraestrutura.Db;
// using MinimalApi.DTOs;
// using MinimalApi.Dominio.Interfaces;
// using MinimalApi.Dominio.Servicos;
// using MinimalApi.Dominio.ModelViews;
// using MinimalApi.Dominio.Entidades;
// using MinimalApi.Dominio.Enus;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using Microsoft.OpenApi.Models;

// var builder = WebApplication.CreateBuilder(args);

// // ==================== JWT ====================
// var key = "minimal-api-alunos-vamos_la-2026-chavesuperlongaeparasegura123456789";
// var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = securityKey,
//         ValidateIssuer = false,
//         ValidateAudience = false,
//         ValidateLifetime = true,
//         ClockSkew = TimeSpan.Zero
//     };
// });

// builder.Services.AddAuthorization();
// builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
// builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();
// builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddSwaggerGen(options =>
// {
//     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         Scheme = "bearer",
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Description = "Insira o token JWT desta forma: Bearer {seu token}"
//     });

//     options.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
//             },
//             Array.Empty<string>()
//         }
//     });
// });

// builder.Services.AddDbContext<DbContexto>(options =>
// {
//     var stringConexao = builder.Configuration.GetConnectionString("Mysql");
//     options.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
// });

// var app = builder.Build();

// // --- MANTENHA ESSA ORDEM ---
// app.UseSwagger();
// app.UseSwaggerUI();
// app.UseAuthentication();
// app.UseAuthorization();
// // ---------------------------

// #region Endpoints
// app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");

// string GerarTokenJwt(Administrador administrador)
// {
//     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
//     var claims = new List<Claim>
//     {
//         new Claim("Email", administrador.Email),
//         new Claim("Perfil", administrador.Perfil ?? "Editor")
//     };
//     var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(2), signingCredentials: credentials);
//     return new JwtSecurityTokenHandler().WriteToken(token);
// }

// app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
// {
//     var adm = administradorServico.Login(loginDTO);
//     if (adm is null) return Results.Unauthorized();
//     return Results.Ok(new AdministradorLogado { Email = adm.Email, Perfil = adm.Perfil, Token = GerarTokenJwt(adm) });
// }).AllowAnonymous().WithTags("Administradores");

// app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
// {
//     return Results.Ok(administradorServico.Todos(pagina).Select(adm => new AdministradorModelView { Id = adm.Id, Email = adm.Email, Perfil = adm.Perfil }));
// }).RequireAuthorization().WithTags("Administradores");

// app.MapPost("/seed-admin", (IAdministradorServico servico) =>
// {
//     servico.Incluir(new Administrador { Email = "administrador@teste.com", Senha = "123456", Perfil = "Admin" });
//     return Results.Ok("Administrador de teste criado com sucesso!");
// }).AllowAnonymous().WithTags("Seed");
// #endregion

// app.Run();

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.DTOs;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ==================== JWT ====================
var key = "minimal-api-alunos-vamos_la-2026-chavesuperlongaeparasegura123456789";
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = securityKey,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta forma: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<DbContexto>(options =>
{
    var stringConexao = builder.Configuration.GetConnectionString("Mysql");
    options.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Administradores
string GerarTokenJwt(Administrador administrador)
{
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    var claims = new List<Claim>
    {
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.Perfil ?? "Editor")
    };
    var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(2), signingCredentials: credentials);
    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    var adm = administradorServico.Login(loginDTO);
    if (adm is null) return Results.Unauthorized();
    return Results.Ok(new AdministradorLogado { Email = adm.Email, Perfil = adm.Perfil, Token = GerarTokenJwt(adm) });
}).AllowAnonymous().WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
{
    return Results.Ok(administradorServico.Todos(pagina).Select(adm => new AdministradorModelView { Id = adm.Id, Email = adm.Email, Perfil = adm.Perfil }));
}).RequireAuthorization().WithTags("Administradores");

app.MapGet("/administradores/{id}", (int id, IAdministradorServico administradorServico) =>
{
    var adm = administradorServico.BuscaPorId(id);
    return adm is not null ? Results.Ok(new AdministradorModelView { Id = adm.Id, Email = adm.Email, Perfil = adm.Perfil }) : Results.NotFound();
}).RequireAuthorization().WithTags("Administradores");

app.MapDelete("/administradores/{id}", (int id, IAdministradorServico administradorServico) =>
{
    var adm = administradorServico.BuscaPorId(id);
    if (adm is null) return Results.NotFound();
    
    administradorServico.Apagar(adm);
    return Results.NoContent();
}).RequireAuthorization().WithTags("Administradores");
#endregion

#region Veiculos
app.MapGet("/veiculos", ([FromQuery] int? pagina, [FromQuery] string? nome, [FromQuery] string? marca, IVeiculoServico veiculoServico) =>
{
    return Results.Ok(veiculoServico.Todos(pagina, nome, marca));
}).RequireAuthorization().WithTags("Veiculos");

app.MapGet("/veiculos/{id}", (int id, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.BuscaPorId(id);
    return veiculo is not null ? Results.Ok(veiculo) : Results.NotFound();
}).RequireAuthorization().WithTags("Veiculos");

app.MapPost("/veiculos", (VeiculoDTO veiculoDto, IVeiculoServico veiculoServico) =>
{
    var veiculo = new Veiculo { Nome = veiculoDto.Nome, Marca = veiculoDto.Marca, Ano = veiculoDto.Ano };
    veiculoServico.Incluir(veiculo);
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
}).RequireAuthorization().WithTags("Veiculos");

app.MapPut("/veiculos/{id}", (int id, VeiculoDTO veiculoDto, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.BuscaPorId(id);
    if (veiculo is null) return Results.NotFound();
    
    veiculo.Nome = veiculoDto.Nome;
    veiculo.Marca = veiculoDto.Marca;
    veiculo.Ano = veiculoDto.Ano;
    
    veiculoServico.Atualizar(veiculo);
    return Results.Ok(veiculo);
}).RequireAuthorization().WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", (int id, IVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.BuscaPorId(id);
    if (veiculo is null) return Results.NotFound();
    
    veiculoServico.Apagar(veiculo);
    return Results.NoContent();
}).RequireAuthorization().WithTags("Veiculos");
#endregion

#region Seed
app.MapPost("/seed-admin", (IAdministradorServico servico) =>
{
    servico.Incluir(new Administrador { Email = "administrador@teste.com", Senha = "123456", Perfil = "Admin" });
    return Results.Ok("Administrador de teste criado com sucesso!");
}).AllowAnonymous().WithTags("Seed");
#endregion

app.Run();