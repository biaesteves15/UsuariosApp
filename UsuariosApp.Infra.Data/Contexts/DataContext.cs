using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Infra.Data.Contexts
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("DbUsuarios");
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Perfil> Perfis { get; set; }
    }
}
