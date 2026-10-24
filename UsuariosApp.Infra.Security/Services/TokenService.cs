using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Infra.Security.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime ExpiraEm) Gerar(Usuario usuario)
        {
            #region Ler as configurações do appsettings.json

            var issuer = _configuration["Jwt:Issuer"]!;
            var audience = _configuration["Jwt:Audience"]!;
            var expirationInMinutes = _configuration["Jwt:ExpirationInMinutes"]!;
            var secretKey = _configuration["Jwt:SecretKey"]!;

            #endregion

            #region Calculando a data e hora de expiração do token

            var expiraEm = DateTime.UtcNow.AddMinutes(int.Parse(expirationInMinutes));

            #endregion

            #region Informações do usuário armazenadas no JWT

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Perfil?.Nome!),
            };

            #endregion

            #region Configurando a assinatura do Token (chave criptografada)

            var credentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    SecurityAlgorithms.HmacSha256
                );

            #endregion

            #region Criando o token JWT

            var jwt = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: expiraEm,
                    signingCredentials: credentials
                );

            #endregion

            #region Convertendo o token como string

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            #endregion

            #region Retornando o token e a data de expiração

            return (token, expiraEm);

            #endregion
        }
    }
}
