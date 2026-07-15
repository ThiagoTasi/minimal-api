using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos;

public class AdministradorServico : IAdministradorServico
{
    private readonly DbContexto _contexto;

    public AdministradorServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public Administrador? Login(LoginDTO loginDTO)
    // {
    //     return _contexto.Administradores.FirstOrDefault(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
    // }
    {
    // Log para ver o que estamos recebendo do formulário
    Console.WriteLine($"DEBUG: Recebido Email: '{loginDTO.Email}', Senha: '{loginDTO.Senha}'");

    var adm = _contexto.Administradores.FirstOrDefault(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);

    if (adm == null)
    {
        // Log para ver se ele encontrou o usuário pelo menos pelo email
        var usuarioPorEmail = _contexto.Administradores.FirstOrDefault(a => a.Email == loginDTO.Email);
        if (usuarioPorEmail != null)
        {
            Console.WriteLine($"DEBUG: Usuário encontrado pelo email, mas a senha no banco é: '{usuarioPorEmail.Senha}'");
        }
        else
        {
            Console.WriteLine("DEBUG: Usuário nem foi encontrado pelo email.");
        }
    }
    else
    {
        Console.WriteLine("DEBUG: Login realizado com sucesso no banco!");
    }

    return adm;
}

    public Administrador Incluir(Administrador administrador)
    {
        _contexto.Administradores.Add(administrador);
        _contexto.SaveChanges();
        return administrador;
    }

    public void Atualizar(Administrador administrador)
    {
        _contexto.Administradores.Update(administrador);
        _contexto.SaveChanges();
    }

    public void Apagar(Administrador administrador)
    {
        _contexto.Administradores.Remove(administrador);
        _contexto.SaveChanges();
    }

    public Administrador? BuscaPorId(int id)
    {
        return _contexto.Administradores.Find(id);
    }

    public List<Administrador> Todos(int? pagina = 1)
    {
        int itensPorPagina = 10;
        int paginaAtual = pagina ?? 1;
        
        return _contexto.Administradores
            .Skip((paginaAtual - 1) * itensPorPagina)
            .Take(itensPorPagina)
            .ToList();
    }
}