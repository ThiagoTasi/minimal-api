// using System.Text;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.OpenApi.Models;
// using MinimalApi.Dominio.Entidades;
// using MinimalApi.Dominio.Interfaces;
// using MinimalApi.Dominio.ModelViews;
// using MinimalApi.Dominio.Servicos;
// using MinimalApi.DTOs;
// using MinimalApi.Infraestrutura.Db;

// public class Startup
// {
//     public Startup(IConfiguration configuration)
//     {
//         Configuration = configuration;
//     }

//     public IConfiguration Configuration { get; set; }

//     public void ConfigureServices(IServiceCollection services)
//     {
//         // Configuração JWT
//         var key = "minimal-api-alunos-vamos_la-2026-chavesuperlongaeparasegura123456789";
//         var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

//         services.AddAuthentication(options =>
//         {
//             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         })
//         .AddJwtBearer(options =>
//         {
//             options.TokenValidationParameters = new TokenValidationParameters
//             {
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey = securityKey,
//                 ValidateAudience = false,
//                 ValidateIssuer = false,
//                 ValidateLifetime = true,
//                 ClockSkew = TimeSpan.Zero
//             };
//         });

//         services.AddAuthorization();
//         services.AddScoped<IAdministradorServico, AdministradorServico>();
//         services.AddScoped<IVeiculoServico, VeiculoServico>();
//         services.AddEndpointsApiExplorer();

//         services.AddSwaggerGen(options =>
//         {
//             options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//             {
//                 Name = "Authorization",
//                 Type = SecuritySchemeType.Http,
//                 Scheme = "bearer",
//                 BearerFormat = "JWT",
//                 In = ParameterLocation.Header
//             });

//             options.AddSecurityRequirement(new OpenApiSecurityRequirement 
//             { 
//                 { 
//                     new OpenApiSecurityScheme 
//                     { 
//                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } 
//                     }, 
//                     Array.Empty<string>() 
//                 } 
//             });
//         });

//         services.AddDbContext<DbContexto>(options =>
//         {
//             var stringConexao = Configuration.GetConnectionString("Mysql");
//             options.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
//         });
//     }

//     public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//     {
//         app.UseSwagger();
//         app.UseSwaggerUI();
//         app.UseRouting();

//         app.UseAuthentication();
//         app.UseAuthorization();



//         app.UseEndpoints(endpoints =>
//         {
//             #region Home
//             endpoints.MapGet("/", () => Results.Json(new { mensagem = "Bem vindo à API" }))
//                      .AllowAnonymous()
//                      .WithTags("Home");
//             #endregion

//             #region Administradores
//             endpoints.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
//             {
//                 var adm = administradorServico.Login(loginDTO);
//                 if (adm is null) return Results.Unauthorized();
//                 return Results.Ok(new AdministradorLogado { Email = adm.Email, Perfil = adm.Perfil, Token = GerarTokenJwt(adm) });
//             }).AllowAnonymous().WithTags("Administradores");

//             endpoints.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
//             {
//                 return Results.Ok(administradorServico.Todos(pagina).Select(adm => new AdministradorModelView { Id = adm.Id, Email = adm.Email, Perfil = adm.Perfil }));
//             }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Administradores");

//             endpoints.MapPost("/administradores", (AdministradorDTO administradorDto, IAdministradorServico administradorServico) =>
//             {
//                 var adm = new Administrador { Email = administradorDto.Email, Senha = administradorDto.Senha, Perfil = administradorDto.Perfil.ToString()! };
//                 administradorServico.Incluir(adm);
//                 return Results.Created($"/administradores/{adm.Id}", adm);
//             }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Administradores");

//             endpoints.MapGet("/administradores/{id}", (int id, IAdministradorServico administradorServico) =>
//             {
//                 var adm = administradorServico.BuscaPorId(id);
//                 return adm is not null ? Results.Ok(new AdministradorModelView { Id = adm.Id, Email = adm.Email, Perfil = adm.Perfil }) : Results.NotFound();
//             }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Administradores");
//             #endregion

//             #region Veiculos
//             endpoints.MapGet("/veiculos", ([FromQuery] int? pagina, [FromQuery] string? nome, [FromQuery] string? marca, IVeiculoServico veiculoServico) =>
//             {
//                 return Results.Ok(veiculoServico.Todos(pagina, nome, marca));
//             }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Editor" }).WithTags("Veiculos");

//             endpoints.MapGet("/veiculos/{id}", (int id, IVeiculoServico veiculoServico) =>
//             {
//                 var veiculo = veiculoServico.BuscaPorId(id);
//                 return veiculo is not null ? Results.Ok(veiculo) : Results.NotFound();
//             }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Editor" }).WithTags("Veiculos");

//             endpoints.MapPost("/veiculos", (VeiculoDTO veiculoDto, IVeiculoServico veiculoServico) =>
//             {
//                 var veiculo = new Veiculo { Nome = veiculoDto.Nome, Marca = veiculoDto.Marca, Ano = veiculoDto.Ano };
//                 veiculoServico.Incluir(veiculo);
//                 return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
//             }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Editor" }).WithTags("Veiculos");

//             endpoints.MapPut("/veiculos/{id}", (int id, VeiculoDTO veiculoDto, IVeiculoServico veiculoServico) =>
//             {
//                 var veiculo = veiculoServico.BuscaPorId(id);
//                 if (veiculo is null) return Results.NotFound();
//                 veiculo.Nome = veiculoDto.Nome;
//                 veiculo.Marca = veiculoDto.Marca;
//                 veiculo.Ano = veiculoDto.Ano;
//                 veiculoServico.Atualizar(veiculo);
//                 return Results.Ok(veiculo);
//             }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Veiculos");

//             endpoints.MapDelete("/veiculos/{id}", (int id, IVeiculoServico veiculoServico) =>
//             {
//                 var veiculo = veiculoServico.BuscaPorId(id);
//                 if (veiculo is null) return Results.NotFound();
//                 veiculoServico.Apagar(veiculo);
//                 return Results.NoContent();
//             }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Veiculos");
//             #endregion
//         });
//     }
// }

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.Db;

public class Startup
{
    private string key = "SuaChaveSecretaMuitoSeguraAqui"; // Substitua pela sua chave do appsettings
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // 1. Configuração de Autenticação JWT
        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddAuthorization();
        
        // 2. Injeção de Dependências
        services.AddScoped<IAdministradorServico, AdministradorServico>();
        services.AddScoped<IVeiculoServico, VeiculoServico>();

        // 3. Banco de Dados e Swagger
        // services.AddDbContext<DbContexto>(options => {
        //     var stringConexao = Configuration.GetConnectionString("mysql");
        //     options.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
        // });

        // Substitua o trecho do DbContext por este:
    services.AddDbContext<DbContexto>(options => {
    options.UseSqlite("Data Source=minimal_api.db");
    
    });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                In = ParameterLocation.Header
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] {} }
            });
        });
    }

    // Método para gerar o token, centralizado aqui na classe
    public string GerarTokenJwt(Administrador administrador)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
            new Claim("Email", administrador.Email),
            new Claim("Perfil", administrador.Perfil),
            new Claim(ClaimTypes.Role, administrador.Perfil)
        };
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
{
    // --- Região Administradores ---
    endpoints.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
    {
        var adm = administradorServico.Login(loginDTO);
        if (adm != null) return Results.Ok(new AdministradorLogado { Email = adm.Email, Perfil = adm.Perfil, Token = GerarTokenJwt(adm) });
        return Results.Unauthorized();
    }).AllowAnonymous().WithTags("Administradores");

    endpoints.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
    {
        // Validação Manual (como nos prints)
        var validacao = new ErrosDeValidacao { Mensagens = new List<string>() };
        if (string.IsNullOrEmpty(administradorDTO.Email)) validacao.Mensagens.Add("Email não pode ser vazio");
        if (string.IsNullOrEmpty(administradorDTO.Senha)) validacao.Mensagens.Add("Senha não pode ser vazia");
        if (administradorDTO.Perfil == null) validacao.Mensagens.Add("Perfil não pode ser vazio");
        
        if (validacao.Mensagens.Count > 0) return Results.BadRequest(validacao);

        var administrador = new Administrador { Email = administradorDTO.Email, Senha = administradorDTO.Senha, Perfil = administradorDTO.Perfil?.ToString() ?? ""};
        administradorServico.Incluir(administrador);
        return Results.Created($"/administradores/{administrador.Id}", administrador);
    }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

    // --- Região Veículos ---
    endpoints.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
    {
        // Validação Manual (como no print 17/18)
        var validacao = new ErrosDeValidacao { Mensagens = new List<string>() };
        if (string.IsNullOrEmpty(veiculoDTO.Nome)) validacao.Mensagens.Add("O nome não pode ser vazio");
        if (string.IsNullOrEmpty(veiculoDTO.Marca)) validacao.Mensagens.Add("A Marca não pode ficar em branco");
        if (veiculoDTO.Ano < 1950) validacao.Mensagens.Add("Veículo muito antigo, aceitamos apenas acima de 1950");
        
        if (validacao.Mensagens.Count > 0) return Results.BadRequest(validacao);

        var veiculo = new Veiculo { Nome = veiculoDTO.Nome, Marca = veiculoDTO.Marca, Ano = veiculoDTO.Ano };
        veiculoServico.Incluir(veiculo);
        return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
    }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Veículos");

    endpoints.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoServico veiculoServico) =>
    {
        var veiculos = veiculoServico.Todos(pagina);
        return Results.Ok(veiculos);
    }).RequireAuthorization().WithTags("Veículos");

    endpoints.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>
    {
        var veiculo = veiculoServico.BuscaPorId(id);
        if (veiculo == null) return Results.NotFound();
        return Results.Ok(veiculo);
    }).RequireAuthorization().WithTags("Veículos");

    endpoints.MapPut("/veiculos/{id}", ([FromRoute] int id, [FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
    {
        var veiculo = veiculoServico.BuscaPorId(id);
        if (veiculo == null) return Results.NotFound();
        
        // Validação na atualização
        var validacao = new ErrosDeValidacao { Mensagens = new List<string>() };
        if (string.IsNullOrEmpty(veiculoDTO.Nome)) validacao.Mensagens.Add("O nome não pode ser vazio");
        if (validacao.Mensagens.Count > 0) return Results.BadRequest(validacao);

        veiculo.Nome = veiculoDTO.Nome;
        veiculo.Marca = veiculoDTO.Marca;
        veiculo.Ano = veiculoDTO.Ano;
        veiculoServico.Atualizar(veiculo);
        return Results.Ok(veiculo);
    }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Veículos");

    endpoints.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>
    {
        var veiculo = veiculoServico.BuscaPorId(id);
        if (veiculo == null) return Results.NotFound();
        veiculoServico.Apagar(veiculo);
        return Results.NoContent();
    }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Veículos");
});
    }
}