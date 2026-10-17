using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Domain.Contracts
{
    public interface IPerfilRepository
    {
        Task Adicionar(Perfil perfil);
        Task<Perfil?> Obter(string nome);
    }
}
