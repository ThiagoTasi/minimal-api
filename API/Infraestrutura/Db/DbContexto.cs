using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Adicionado para reconhecer o IConfiguration corretamente
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Infraestrutura.Db;

public class DbContexto : DbContext
{
    private readonly IConfiguration _configuracaoAppSettings;

    public DbContexto(IConfiguration configuracaoAppSettings)
    {
        _configuracaoAppSettings = configuracaoAppSettings;
    }

    public DbContexto(DbContextOptions<DbContexto> options, IConfiguration configuracaoAppSettings = null!) : base(options)
    {
        _configuracaoAppSettings = configuracaoAppSettings;
    }
    public DbSet<Administrador> Administradores { get; set; } = default!;
     public DbSet<Veiculo> Veiculos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    //     // Correção de sintaxe aplicada aqui:
    //     modelBuilder.Entity<Administrador>().HasData(
    //         new Administrador 
    //         {
    //             Id = 1, // O EF precisa do ID fixo para o Seed de dados funcionar
    //             Email = "administrador@teste.com",
    //             Senha = "123456",
    //             Perfil = "Adm"
    //         }
    //     ); // Fechamos corretamente com );
    // }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         // Mandou muito bem abrindo e organizando as chaves do IF aqui!
//         if (!optionsBuilder.IsConfigured)
//         {
//             var stringConexao = _configuracaoAppSettings.GetConnectionString("Mysql")?.ToString();
            
//             if (!string.IsNullOrEmpty(stringConexao))
//             {
//                 optionsBuilder.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
//             }
//         }
//     }
// }
     //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         if (!optionsBuilder.IsConfigured)
//         {
//             // Para mudar de banco, basta trocar esta linha aqui:
//             // optionsBuilder.UseSqlite("Data Source=minimal_api.db");
            
//             // Para usar o MySQL, descarte a linha acima e use as de baixo:
//             var stringConexao = _configuracaoAppSettings.GetConnectionString("Mysql");
//             if (!string.IsNullOrEmpty(stringConexao))
//             {
//                 //optionsBuilder.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
//                 optionsBuilder.UseSqlite("Data Source=novo_banco_limpo.db");
//             }
//         }
//     }

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        // Forçando o uso do SQLite com o novo nome de arquivo
        optionsBuilder.UseSqlite("Data Source=novo_banco_limpo.db");
    }
}
// }

    
    

