using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosApp.Api.Controllers
{
    [Route("api/v1/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        [HttpPost("criar")]
        public async Task<IActionResult> CriarAsync([FromBody] Dictionary<string, object> request)
        {
            return StatusCode(502, new
            {
                status = 502,
                message = "Não implementado."
            });
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
