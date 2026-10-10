using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http.Json;

namespace UsuariosApp.Tests.IntegrationTests
{
    public class UsuariosControllerTest : IDisposable
    {
        private readonly HttpClient _client;
        private readonly Faker _faker;
        private readonly WebApplicationFactory<Program> _factory;

        public UsuariosControllerTest()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            _faker = new Faker("pt_BR");
        }

        private const string EndpointCriar = "/api/v1/usuarios/criar";
        private const string EndpointAutenticar = "/api/v1/usuarios/autenticar";

        [Fact(
            DisplayName = "Deve criar um usuário com dados válidos na API."
        )]
        public async Task DeveCriarUsuarioComDadosValidos()
        {
            #region Arrange

            var novoUsuario = new
            {
                nome = _faker.Name.FullName(),
                email = _faker.Internet.Email(),
                senha = "Teste@123456"
            };

            #endregion

            #region Act

            var response = await _client.PostAsJsonAsync(
                EndpointCriar, novoUsuario);

            #endregion

            #region Assert

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);

            #endregion
        }

        [Fact(
            DisplayName = "Deve retornar 400 ao criar usuário com dados inválidos na API."
        )]
        public async Task DeveRetornar400AoCriarUsuarioComDadosInvalidos()
        {
            #region Arrange

            var novoUsuario = new
            {
                nome = "",
                email = "email-invalido",
                senha = ""
            };

            #endregion

            #region Act

            var response = await _client.PostAsJsonAsync(
                EndpointCriar, novoUsuario);

            #endregion

            #region Assert

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);

            #endregion
        }

        [Fact(
            DisplayName = "Deve retornar 409 ao criar usuário com email duplicado na API."
        )]
        public async Task DeveRetornar409AoCriarUsuarioComEmailDuplicado()
        {
            #region Arrange

            var novoUsuario = new
            {
                nome = _faker.Name.FullName(),
                email = _faker.Internet.Email(),
                senha = "Teste@123456"
            };

            // Cadastrando o primeiro usuário
            var primeiroCadastro = await _client.PostAsJsonAsync(
                EndpointCriar, novoUsuario);

            primeiroCadastro.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);

            #endregion

            #region Act

            // Tentando cadastrar novamente o mesmo email
            var response = await _client.PostAsJsonAsync(
                EndpointCriar, novoUsuario);

            #endregion

            #region Assert

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Conflict);

            #endregion
        }

        [Fact(
            DisplayName = "Deve autenticar usuário com credenciais válidas."
        )]
        public async Task DeveAutenticarUsuarioComCredenciaisValidas()
        {
            #region Arrange

            var novoUsuario = new
            {
                nome = _faker.Name.FullName(),
                email = _faker.Internet.Email(),
                senha = "Teste@123456"
            };

            // Cadastrando o usuário antes da autenticação
            var cadastroResponse = await _client.PostAsJsonAsync(
                EndpointCriar, novoUsuario);

            cadastroResponse.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);

            var login = new
            {
                email = novoUsuario.email,
                senha = novoUsuario.senha
            };

            #endregion

            #region Act

            var response = await _client.PostAsJsonAsync(
                EndpointAutenticar, login);

            #endregion

            #region Assert

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            #endregion
        }

        [Fact(
            DisplayName = "Deve retornar 401 ao autenticar usuário inexistente."
        )]
        public async Task DeveRetornar401AoAutenticarUsuarioInexistente()
        {
            #region Arrange

            var login = new
            {
                email = $"inexistente_{Guid.NewGuid():N}@teste.com",
                senha = "Teste@123456"
            };

            #endregion

            #region Act

            var response = await _client.PostAsJsonAsync(
                EndpointAutenticar, login);

            #endregion

            #region Assert

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);

            #endregion
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}