using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Application.Dtos
{
    public record CriarUsuarioResponse(
            Guid id,
            DateTime dataHora,
            string mensagem
        );
}