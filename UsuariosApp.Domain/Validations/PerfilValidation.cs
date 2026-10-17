using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Domain.Validations
{
    public class PerfilValidation : AbstractValidator<Perfil>
    {
        public PerfilValidation()
        {
            RuleFor(p => p.Id)
                .NotNull().WithMessage("O ID do perfil é obrigatório.");

            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome do perfil é obrigatório.")
                .MaximumLength(50).WithMessage("O nome do perfil deve ter no máximo 50 caracteres.")
                .MinimumLength(5).WithMessage("O nome do perfil deve ter no mínimo 5 caracteres.");
        }
    }
}
