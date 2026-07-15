using MinimalApi.Dominio.Entidades;
using MinimalApi.DTOs;

namespace MinimalApi.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        Administrador? Login(LoginDTO loginDTO);
        
        Administrador Incluir(Administrador administrador);
        
        // Adicionando estes dois abaixo para completar seu CRUD
        void Atualizar(Administrador administrador);
        void Apagar(Administrador administrador);
        
        Administrador? BuscaPorId(int id);
        
        List<Administrador> Todos(int? pagina = 1);
    }
}
