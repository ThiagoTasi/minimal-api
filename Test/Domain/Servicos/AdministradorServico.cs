// using MinimalApi.Dominio.Entidades;
// using MinimalApi.Infraestrutura.Db;
// using Microsoft.Extensions.Configuration;
// using Microsoft.EntityFrameworkCore;

// namespace Test.Domain.Entidades;

// [TestClass]
// public class AdministradorServicoTest
// {
//     private DbContexto CriarContextoDeTeste()
//     {
//         var builder = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//             .AddEnvironmentVariables();

//         var configuration = builder.Build();
        
//         var connectionString = configuration.GetConnectionString("MySql");

//         var options = new DbContextOptionsBuilder<DbContexto>()
//             .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
//             .Options;

//         return new DbContexto(options);
//     }

//     [TestMethod]
//     public void TestandoSalvarAdministrador()
//     {
//         // Arrange
//         var context = CriarContextoDeTeste();
//         context.Database.ExecuteSqlRaw("TRUNCATE TABLE administradores"); // Limpa a tabela antes do teste

//         var adm = new Administrador();
//         adm.Id = 1;
//         adm.Email = "teste@teste.com";
//         adm.Senha = "teste";
//         adm.Perfil = "Adm";

//         // Act
//         context.Administradores.Add(adm);
//         context.SaveChanges();

//         // Assert
//         var admDoBanco = context.Administradores.FirstOrDefault(a => a.Email == "teste@teste.com");
//         Assert.IsNotNull(admDoBanco);
//         Assert.AreEqual("teste@teste.com", admDoBanco.Email);
//     }
// }


// using MinimalApi.Dominio.Entidades;
// using MinimalApi.Dominio.Servicos; // Certifique-se de importar o namespace do seu serviço
// using MinimalApi.Infraestrutura.Db;
// using Microsoft.Extensions.Configuration;
// using Microsoft.EntityFrameworkCore;
// using System.Reflection;

// namespace Test.Domain.Entidades;

// [TestClass]
// public class AdministradorServicoTest
// {
//     private DbContexto CriarContextoDeTeste()
//     {
//         var builder = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//             .AddEnvironmentVariables();

//         var configuration = builder.Build();

//         // Se o seu DbContexto exige 'options' (padrão do EF), mantenha assim:
//         var connectionString = configuration.GetConnectionString("MySql");
//         var options = new DbContextOptionsBuilder<DbContexto>()
//             .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
//             .Options;

//         return new DbContexto(options);
//     }

//     [TestMethod]
//     public void TestandoSalvarAdministrador()
//     {
//         // Arrange
//         var context = CriarContextoDeTeste();
//         context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores"); // Ajustado para o nome com 'A' maiúsculo conforme o print

//         var adm = new Administrador();
//         adm.Id = 1;
//         adm.Email = "teste@teste.com";
//         adm.Senha = "teste";
//         adm.Perfil = "Adm";

//         var administradorServico = new AdministradorServico(context);

//         // Act
//         administradorServico.Incluir(adm);
//         var admDoBanco = administradorServico.BuscaPorId(adm.Id);

//         // Assert
//         // Verifica se a contagem após a inclusão é igual a 1
//         //Assert.AreEqual(1, administradorServico.Todos(1).Count());
//         Assert.AreEqual(1, admDoBanco.Id);
//     }
// }

using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Test.Domain.Entidades;

[TestClass]
public class AdministradorServicoTest
{
    private DbContexto CriarContextoDeTeste()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        var connectionString = configuration.GetConnectionString("MySql");
        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;

        return new DbContexto(options);
    }

    [TestMethod]
    public void TestandoSalvarAdministrador()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

        var adm = new Administrador();
        adm.Id = 1;
        adm.Email = "teste@teste.com";
        adm.Senha = "teste";
        adm.Perfil = "Adm";

        var administradorServico = new AdministradorServico(context);

        // Act
        administradorServico.Incluir(adm);
        var admDoBanco = administradorServico.BuscaPorId(adm.Id);

        // Assert
        Assert.IsNotNull(admDoBanco);
        Assert.AreEqual(1, admDoBanco.Id);
    }
}