using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Contracts;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Infra.Data.Contexts;

namespace UsuariosApp.Infra.Data.Repositories
{
    public class PerfilRepository : IPerfilRepository
    {
        private readonly DataContext _context;

        public PerfilRepository(DataContext context)
        {
            _context = context;
        }

        public async Task Adicionar(Perfil perfil)
        {
            await _context.Perfis.AddAsync(perfil);
            await _context.SaveChangesAsync();
        }

        public async Task<Perfil?> Obter(string nome)
        {
            return await _context.Perfis
                    .FirstOrDefaultAsync(p => p.Nome.Equals(nome));
        }
    }
}
