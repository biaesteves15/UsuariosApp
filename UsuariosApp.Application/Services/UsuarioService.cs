using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Application.Dtos;
using UsuariosApp.Domain.Contracts;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Domain.Validations;
using UsuariosApp.Infra.Security.Services;

namespace UsuariosApp.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPerfilRepository _perfilRepository;
        private readonly TokenService _tokenService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPerfilRepository perfilRepository, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _perfilRepository = perfilRepository;
            _tokenService = tokenService;
        }

        public async Task<AutenticarUsuarioResponse> Autenticar(AutenticarUsuarioRequest request)
        {
            #region Consultando o usuário através do email e da senha informados

            var usuario = await _usuarioRepository.Obter(request.email, request.senha);
            if (usuario == null)
            {
                throw new ApplicationException("Acesso negado. Usuário inválido.");
            }

            #endregion

            #region Gerando o token JWT e a data de expiração

            var (token, expiraEm) = _tokenService.Gerar(usuario);

            #endregion

            #region Retornar os dados do usuário autenticado

            return new AutenticarUsuarioResponse(
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    usuario.Perfil?.Nome!,
                    token,
                    expiraEm
                );

            #endregion

        }

        public async Task<CriarUsuarioResponse> Criar(CriarUsuarioRequest request)
        {
            //Capturando os dados recebidos do request
            var usuario = new Usuario
            {
                Nome = request.nome,
                Email = request.email,
                Senha = request.senha
            };

            //Validar os dados do usuário
            var validation = new UsuarioValidation(_usuarioRepository);
            var result = validation.Validate(usuario);

            //Verificar se há erros de validação
            if (!result.IsValid)
            {
                //Retornar os erros de validação
                throw new ValidationException(result.Errors);
            }

            //Verificar se existe no banco de dados um perfil 'Operador'
            var perfil = await _perfilRepository.Obter("Operador");

            //Se não existir, esse perfil será criado
            if (perfil == null)
            {
                perfil = new Perfil { Nome = "Operador" };
                await _perfilRepository.Adicionar(perfil);
            }

            //Associar o usuário ao perfil
            usuario.PerfilId = perfil.Id;

            //Gravando o usuário no banco de dados
            await _usuarioRepository.Adicionar(usuario);

            //Retornar os dados
            return new CriarUsuarioResponse(
                usuario.Id,
                DateTime.Now,
                "Usuário cadastrado com sucesso."
            );
        }
    }
}
