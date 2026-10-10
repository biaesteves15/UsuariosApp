using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Infra.Data.Mappings
{
    public class PerfilMap : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(p => p.Nome)
                .IsUnique();
        }
    }
}
