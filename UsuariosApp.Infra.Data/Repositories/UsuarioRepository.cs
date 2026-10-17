using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Contracts;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Infra.Data.Contexts;

namespace UsuariosApp.Infra.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataContext _context;

        public UsuarioRepository(DataContext context)
        {
            _context = context;
        }

        public async Task Adicionar(Usuario usuario)
        {
            await _context.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistente(string email)
        {
            var result = await _context.Usuarios
                            .CountAsync(u => u.Email.Equals(email));

            return result > 0;
        }

        public async Task<Usuario?> Obter(string email, string senha)
        {
            return await _context.Usuarios
                    .Include(u => u.Perfil)
                    .FirstOrDefaultAsync(u => u.Email.Equals(email)
                                           && u.Senha.Equals(senha));
        }
    }
}
