using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Application.Dtos
{
    public record AutenticarUsuarioRequest
    (
        string email,
        string senha
    );
}
