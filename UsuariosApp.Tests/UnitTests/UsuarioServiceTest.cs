using FluentAssertions;
using FluentValidation;
using UsuariosApp.Application.Dtos;
using UsuariosApp.Application.Services;
using UsuariosApp.Domain.Contracts;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Tests.UnitTests;

public class UsuarioServiceTest
{
    [Fact(DisplayName = "Deve cadastrar usuário e associá-lo ao perfil existente.")]
    public async Task DeveCadastrarUsuarioComPerfilExistente()
    {
        var perfil = new Perfil { Nome = "Operador" };
        var usuarioRepository = new UsuarioRepositoryFake();
        var perfilRepository = new PerfilRepositoryFake(perfil);
        var service = new UsuarioService(usuarioRepository, perfilRepository);

        var response = await service.Criar(CriarRequest());

        usuarioRepository.UsuarioAdicionado.Should().NotBeNull();
        usuarioRepository.UsuarioAdicionado!.PerfilId.Should().Be(perfil.Id);
        usuarioRepository.UsuarioAdicionado.Nome.Should().Be("Ana Silva");
        usuarioRepository.UsuarioAdicionado.Email.Should().Be("ana.silva@teste.com");
        usuarioRepository.UsuarioAdicionado.Senha.Should().Be("Senha@123");
        usuarioRepository.AdicionarChamadas.Should().Be(1);
        perfilRepository.ObterChamadas.Should().Be(1);
        perfilRepository.PerfilAdicionado.Should().BeNull();
        response.id.Should().Be(usuarioRepository.UsuarioAdicionado.Id);
        response.mensagem.Should().Be("Usuário cadastrado com sucesso.");
    }

    [Fact(DisplayName = "Deve criar o perfil Operador quando ele não existir.")]
    public async Task DeveCriarPerfilOperadorQuandoInexistente()
    {
        var usuarioRepository = new UsuarioRepositoryFake();
        var perfilRepository = new PerfilRepositoryFake();
        var service = new UsuarioService(usuarioRepository, perfilRepository);

        await service.Criar(CriarRequest());

        perfilRepository.PerfilAdicionado.Should().NotBeNull();
        perfilRepository.PerfilAdicionado!.Nome.Should().Be("Operador");
        perfilRepository.AdicionarChamadas.Should().Be(1);
        usuarioRepository.UsuarioAdicionado.Should().NotBeNull();
        usuarioRepository.UsuarioAdicionado!.PerfilId.Should().Be(perfilRepository.PerfilAdicionado.Id);
        usuarioRepository.AdicionarChamadas.Should().Be(1);
    }

    [Fact(DisplayName = "Deve rejeitar dados inválidos sem consultar perfil nem adicionar usuário.")]
    public async Task DeveRejeitarDadosInvalidosSemPersistir()
    {
        var usuarioRepository = new UsuarioRepositoryFake();
        var perfilRepository = new PerfilRepositoryFake();
        var service = new UsuarioService(usuarioRepository, perfilRepository);
        var request = new CriarUsuarioRequest("", "email-invalido", "fraca");

        var act = () => service.Criar(request);

        await act.Should().ThrowAsync<ValidationException>();
        usuarioRepository.UsuarioAdicionado.Should().BeNull();
        usuarioRepository.AdicionarChamadas.Should().Be(0);
        perfilRepository.ObterChamadas.Should().Be(0);
        perfilRepository.PerfilAdicionado.Should().BeNull();
    }

    private static CriarUsuarioRequest CriarRequest() =>
        new("Ana Silva", "ana.silva@teste.com", "Senha@123");

    private sealed class UsuarioRepositoryFake : IUsuarioRepository
    {
        public Usuario? UsuarioAdicionado { get; private set; }
        public int AdicionarChamadas { get; private set; }

        public Task Adicionar(Usuario usuario)
        {
            AdicionarChamadas++;
            UsuarioAdicionado = usuario;
            return Task.CompletedTask;
        }

        public Task<bool> EmailExistente(string email) => Task.FromResult(false);

        public Task<Usuario?> Obter(string email, string senha) => Task.FromResult<Usuario?>(null);
    }

    private sealed class PerfilRepositoryFake(Perfil? perfil = null) : IPerfilRepository
    {
        public Perfil? PerfilAdicionado { get; private set; }
        public int ObterChamadas { get; private set; }
        public int AdicionarChamadas { get; private set; }

        public Task Adicionar(Perfil perfil)
        {
            AdicionarChamadas++;
            PerfilAdicionado = perfil;
            return Task.CompletedTask;
        }

        public Task<Perfil?> Obter(string nome)
        {
            ObterChamadas++;
            return Task.FromResult(perfil);
        }
    }
}
