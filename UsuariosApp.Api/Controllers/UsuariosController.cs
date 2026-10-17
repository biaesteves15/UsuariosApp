using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsuariosApp.Application.Dtos;
using UsuariosApp.Application.Services;

namespace UsuariosApp.Api.Controllers
{
    [Route("api/v1/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("criar")]
        public async Task<IActionResult> CriarAsync([FromBody] CriarUsuarioRequest request)
        {
            try
            {
                var response = await _usuarioService.Criar(request);

                //CREATED (201)
                return StatusCode(201, response);
            }
            catch (ValidationException e)
            {
                //BAD REQUEST (400)
                return BadRequest(e.Errors.Select(e => e.ErrorMessage));
            }
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> AutenticarAsync([FromBody] Dictionary<string, object> request)
        {
            return StatusCode(502, new
            {
                status = 502,
                message = "Não implementado."
            });
        }
    }
}
