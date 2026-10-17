using FluentAssertions;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Domain.Validations;

namespace UsuariosApp.Tests.UnitTests;

public class UsuarioValidationTest
{
    [Fact(DisplayName = "Deve aceitar usuário com dados válidos.")]
    public void DeveAceitarUsuarioComDadosValidos()
    {
        var validator = new UsuarioValidation(new UsuarioRepositoryFake());

        var result = validator.Validate(CriarUsuario());

        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Deve rejeitar nome vazio ou menor que cinco caracteres.")]
    [InlineData("")]
    [InlineData("Abcd")]
    public void DeveRejeitarNomeInvalido(string nome)
    {
        var usuario = CriarUsuario();
        usuario.Nome = nome;

        var result = new UsuarioValidation(new UsuarioRepositoryFake()).Validate(usuario);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(Usuario.Nome));
    }

    [Fact(DisplayName = "Deve aceitar nome de usuário com exatamente cinco caracteres.")]
    public void DeveAceitarNomeNoLimiteMinimo()
    {
        var usuario = CriarUsuario();
        usuario.Nome = "Abcde";

        var result = new UsuarioValidation(new UsuarioRepositoryFake()).Validate(usuario);

        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Deve rejeitar e-mail vazio ou inválido.")]
    [InlineData("")]
    [InlineData("email-invalido")]
    public void DeveRejeitarEmailInvalido(string email)
    {
        var usuario = CriarUsuario();
        usuario.Email = email;

        var result = new UsuarioValidation(new UsuarioRepositoryFake()).Validate(usuario);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(Usuario.Email));
    }

    [Fact(DisplayName = "Deve rejeitar e-mail já cadastrado.")]
    public void DeveRejeitarEmailDuplicado()
    {
        var usuario = CriarUsuario();
        var repository = new UsuarioRepositoryFake(emailExistente: true);

        var result = new UsuarioValidation(repository).Validate(usuario);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "O email informado já está cadastrado para outro usuário.");
    }

    [Theory(DisplayName = "Deve rejeitar senha vazia ou fora dos requisitos.")]
    [InlineData("")]
    [InlineData("senhaa123!")]
    [InlineData("SENHAA123!")]
    [InlineData("Senhaaaaa!")]
    [InlineData("Senha1234")]
    [InlineData("Aa1!abc")]
    [InlineData("Aa1! abc")]
    public void DeveRejeitarSenhaInvalida(string senha)
    {
        var usuario = CriarUsuario();
        usuario.Senha = senha;

        var result = new UsuarioValidation(new UsuarioRepositoryFake()).Validate(usuario);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(Usuario.Senha));
    }

    [Fact(DisplayName = "Deve aceitar senha com exatamente oito caracteres e todos os requisitos.")]
    public void DeveAceitarSenhaNoLimiteMinimo()
    {
        var usuario = CriarUsuario();
        usuario.Senha = "Aa1!abcd";

        var result = new UsuarioValidation(new UsuarioRepositoryFake()).Validate(usuario);

        result.IsValid.Should().BeTrue();
    }

    private static Usuario CriarUsuario() => new()
    {
        Nome = "Ana Silva",
        Email = "ana.silva@teste.com",
        Senha = "Senha@123"
    };

    private sealed class UsuarioRepositoryFake(bool emailExistente = false)
        : UsuariosApp.Domain.Contracts.IUsuarioRepository
    {
        public Task Adicionar(Usuario usuario) => Task.CompletedTask;

        public Task<bool> EmailExistente(string email) => Task.FromResult(emailExistente);

        public Task<Usuario?> Obter(string email, string senha) => Task.FromResult<Usuario?>(null);
    }
}
