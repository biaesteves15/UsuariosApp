using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Application.Dtos
{
    public record AutenticarUsuarioResponse(
        Guid id,
        string nome,
        string email,
        string perfil,
        string token,
        DateTime expiraEm
    );
}