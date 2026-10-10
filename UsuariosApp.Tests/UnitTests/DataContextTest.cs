using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Infra.Data.Contexts;

namespace UsuariosApp.Tests.UnitTests
{
    public class DataContextTest
    {
        [Fact(DisplayName = "Deve configurar o banco de dados em memória.")]
        public void DeveConfigurarBancoDeDadosEmMemoria()
        {
            #region Arrange

            using var context = new DataContext();

            #endregion

            #region Act

            var provider = context.Database.ProviderName;

            #endregion

            #region Assert

            provider.Should()
                .Be("Microsoft.EntityFrameworkCore.InMemory");

            #endregion
        }

        [Fact(DisplayName = "Deve inicializar o DbSet de usuários.")]
        public void DeveInicializarDbSetDeUsuarios()
        {
            #region Arrange

            using var context = new DataContext();

            #endregion

            #region Act

            var usuarios = context.Usuarios;

            #endregion

            #region Assert

            usuarios.Should().NotBeNull();

            usuarios.Should()
                .BeAssignableTo<DbSet<Usuario>>();

            #endregion
        }

        [Fact(DisplayName = "Deve inicializar o DbSet de perfis.")]
        public void DeveInicializarDbSetDePerfis()
        {
            #region Arrange

            using var context = new DataContext();

            #endregion

            #region Act

            var perfis = context.Perfis;

            #endregion

            #region Assert

            perfis.Should().NotBeNull();

            perfis.Should()
                .BeAssignableTo<DbSet<Perfil>>();

            #endregion
        }
    }
}