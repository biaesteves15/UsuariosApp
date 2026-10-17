using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Domain.Contracts
{
    public interface IUsuarioRepository
    {
        Task Adicionar(Usuario usuario);
        Task<bool> EmailExistente(string email);
        Task<Usuario?> Obter(string email, string senha);
    }
}
