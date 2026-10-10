using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Infra.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Senha)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(u => u.Perfil)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(u => u.PerfilId);
        }
    }
}
