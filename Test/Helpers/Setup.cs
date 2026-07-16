// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc.Testing;
// using MinimalApi.Dominio.Interfaces;
// using Test.Mocks;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.EntityFrameworkCore; // Adicione este using
// using MinimalApi.Infraestrutura.Db; // Adicione este using

// namespace Test.Helpers;

// public class Setup
// {
//     public const string PORT = "5001";
//     public static TestContext testContext = default!;
//     public static WebApplicationFactory<Startup> http = default!;
//     public static HttpClient client = default!;

//     public static void ClassInit(TestContext testContext)
//     {
//         Setup.testContext = testContext;
//         Setup.http = new WebApplicationFactory<Startup>();

//         Setup.http = Setup.http.WithWebHostBuilder(builder =>
//         {
//             builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");

//             builder.ConfigureServices(services =>
//             {
//                 services.AddScoped<IAdministradorServico, AdministradorServicoMock>();
//             });
//         });

//         Setup.client = Setup.http.CreateClient();
//     }

//     public static void ClassCleanup()
//     {
//         Setup.http.Dispose();
//     }
// }
     
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using MinimalApi.Dominio.Interfaces;
using Test.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; // Adicione este using
using MinimalApi.Infraestrutura.Db; // Adicione este using

namespace Test.Helpers;

public class Setup
{
    public const string PORT = "5001";
    public static TestContext testContext = default!;
    public static WebApplicationFactory<Startup> http = default!;
    public static HttpClient client = default!;

    public static void ClassInit(TestContext testContext)
    {
        Setup.testContext = testContext;
        Setup.http = new WebApplicationFactory<Startup>();

        Setup.http = Setup.http.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // 1. Remove o contexto original que tenta conectar no MySQL
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DbContexto>));
                if (descriptor != null) services.Remove(descriptor);

                // 2. Adiciona o Banco em Memória (não precisa de conexão externa)
                services.AddDbContext<DbContexto>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // 3. Mantém seu Mock de serviço
                services.AddScoped<IAdministradorServico, AdministradorServicoMock>();
            });
        });

        Setup.client = Setup.http.CreateClient();
    }

    public static void ClassCleanup()
    {
        Setup.http.Dispose();
    }
}
