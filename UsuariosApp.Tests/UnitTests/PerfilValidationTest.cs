using FluentAssertions;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Domain.Validations;

namespace UsuariosApp.Tests.UnitTests;

public class PerfilValidationTest
{
    [Fact(DisplayName = "Deve aceitar perfil com nome entre cinco e cinquenta caracteres.")]
    public void DeveAceitarPerfilComNomeValido()
    {
        var result = new PerfilValidation().Validate(new Perfil { Nome = "Operador" });

        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Deve aceitar nomes de perfil nos limites mínimo e máximo.")]
    [InlineData("Abcde")]
    [InlineData("Abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwx")]
    public void DeveAceitarNomeDePerfilNosLimites(string nome)
    {
        var result = new PerfilValidation().Validate(new Perfil { Nome = nome });

        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Deve rejeitar nome de perfil vazio ou fora dos limites.")]
    [InlineData("")]
    [InlineData("Adm")]
    [InlineData("NomeDePerfilComMaisDeCinquentaCaracteresParaValidarOLimite")]
    public void DeveRejeitarNomeDePerfilInvalido(string nome)
    {
        var result = new PerfilValidation().Validate(new Perfil { Nome = nome });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(Perfil.Nome));
    }
}
