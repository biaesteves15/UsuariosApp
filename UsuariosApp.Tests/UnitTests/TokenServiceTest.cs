using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Infra.Security.Services;

namespace UsuariosApp.Tests.UnitTests;

public class TokenServiceTest
{
    [Fact(DisplayName = "Deve gerar JWT com claims do usuário e expiração configurada.")]
    public void DeveGerarTokenComClaimsEExpiracaoConfiguradas()
    {
        const int expirationInMinutes = 30;
        var usuario = new Usuario
        {
            Nome = "Ana Silva",
            Email = "ana.silva@teste.com",
            Perfil = new Perfil { Nome = "Operador" }
        };
        var tokenService = new TokenService(CriarConfiguracao(expirationInMinutes));

        var (token, expiraEm) = tokenService.Gerar(usuario);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        token.Should().NotBeNullOrWhiteSpace();
        jwt.Issuer.Should().Be("UsuariosApp.Tests");
        jwt.Audiences.Should().Contain("UsuariosApp.Tests.Client");
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == usuario.Id.ToString());
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == usuario.Email);
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == usuario.Id.ToString());
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == usuario.Nome);
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Operador");
        expiraEm.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(expirationInMinutes), TimeSpan.FromSeconds(2));
        jwt.ValidTo.Should().BeCloseTo(expiraEm, TimeSpan.FromSeconds(1));
    }

    private static IConfiguration CriarConfiguracao(int expirationInMinutes) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "UsuariosApp.Tests",
                ["Jwt:Audience"] = "UsuariosApp.Tests.Client",
                ["Jwt:ExpirationInMinutes"] = expirationInMinutes.ToString(),
                ["Jwt:SecretKey"] = "ChaveDeTesteComTamanhoSuficiente123456"
            })
            .Build();
}
