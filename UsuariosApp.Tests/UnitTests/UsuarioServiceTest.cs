using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using UsuariosApp.Application.Dtos;
using UsuariosApp.Application.Services;
using UsuariosApp.Domain.Contracts;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Infra.Security.Services;

namespace UsuariosApp.Tests.UnitTests;

public class UsuarioServiceTest
{
    [Fact(DisplayName = "Deve autenticar usuário encontrado e retornar seus dados e token.")]
    public async Task DeveAutenticarUsuarioEncontrado()
    {
        var usuario = new Usuario
        {
            Nome = "Ana Silva",
            Email = "ana.silva@teste.com",
            Senha = "Senha@123",
            Perfil = new Perfil { Nome = "Operador" }
        };
        var usuarioRepository = new UsuarioRepositoryFake(usuarioAutenticado: usuario);
        var service = new UsuarioService(
            usuarioRepository,
            new PerfilRepositoryFake(),
            CriarTokenService());

        var response = await service.Autenticar(new AutenticarUsuarioRequest(usuario.Email, usuario.Senha));

        usuarioRepository.EmailConsultado.Should().Be(usuario.Email);
        usuarioRepository.SenhaConsultada.Should().Be(usuario.Senha);
        response.id.Should().Be(usuario.Id);
        response.nome.Should().Be(usuario.Nome);
        response.email.Should().Be(usuario.Email);
        response.perfil.Should().Be("Operador");
        response.token.Should().NotBeNullOrWhiteSpace();
        response.expiraEm.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact(DisplayName = "Deve rejeitar autenticação quando o usuário não existe.")]
    public async Task DeveRejeitarAutenticacaoDeUsuarioInexistente()
    {
        var usuarioRepository = new UsuarioRepositoryFake();
        var service = new UsuarioService(
            usuarioRepository,
            new PerfilRepositoryFake(),
            CriarTokenService());

        var act = () => service.Autenticar(new AutenticarUsuarioRequest("ausente@teste.com", "Senha@123"));

        await act.Should()
            .ThrowAsync<ApplicationException>()
            .WithMessage("Acesso negado. Usuário inválido.");
        usuarioRepository.EmailConsultado.Should().Be("ausente@teste.com");
        usuarioRepository.SenhaConsultada.Should().Be("Senha@123");
    }

    [Fact(DisplayName = "Deve cadastrar usuário e associá-lo ao perfil existente.")]
    public async Task DeveCadastrarUsuarioComPerfilExistente()
    {
        var perfil = new Perfil { Nome = "Operador" };
        var usuarioRepository = new UsuarioRepositoryFake();
        var perfilRepository = new PerfilRepositoryFake(perfil);
        var service = new UsuarioService(usuarioRepository, perfilRepository, CriarTokenService());

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
        var service = new UsuarioService(usuarioRepository, perfilRepository, CriarTokenService());

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
        var service = new UsuarioService(usuarioRepository, perfilRepository, CriarTokenService());
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

    private static TokenService CriarTokenService() =>
        new(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "UsuariosApp.Tests",
                ["Jwt:Audience"] = "UsuariosApp.Tests.Client",
                ["Jwt:ExpirationInMinutes"] = "30",
                ["Jwt:SecretKey"] = "ChaveDeTesteComTamanhoSuficiente123456"
            })
            .Build());

    private sealed class UsuarioRepositoryFake(Usuario? usuarioAutenticado = null) : IUsuarioRepository
    {
        public Usuario? UsuarioAdicionado { get; private set; }
        public int AdicionarChamadas { get; private set; }
        public string? EmailConsultado { get; private set; }
        public string? SenhaConsultada { get; private set; }

        public Task Adicionar(Usuario usuario)
        {
            AdicionarChamadas++;
            UsuarioAdicionado = usuario;
            return Task.CompletedTask;
        }

        public Task<bool> EmailExistente(string email) => Task.FromResult(false);

        public Task<Usuario?> Obter(string email, string senha)
        {
            EmailConsultado = email;
            SenhaConsultada = senha;
            return Task.FromResult(usuarioAutenticado);
        }
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
