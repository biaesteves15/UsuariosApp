using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Contracts;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Domain.Validations
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        public UsuarioValidation(IUsuarioRepository usuarioRepository)
        {
            RuleFor(u => u.Id)
                .NotNull().WithMessage("O Id do usuário é obrigatório");

            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
                .MinimumLength(5).WithMessage("O nome do usuário deve ter no mínimo 5 caracteres.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O email do usuário é obrigatório.")
                .EmailAddress().WithMessage("O email deve conter um endereço válido.")
                .Must(email => !usuarioRepository.EmailExistente(email).Result)
                .WithMessage("O email informado já está cadastrado para outro usuário.");

            RuleFor(u => u.Senha)
                .NotEmpty().WithMessage("A senha do usuário é obrigatória.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9\s])\S{8,}$")
                .WithMessage("A senha deve ter pelo menos 1 letra minúscula, 1 letra maiúscula, 1 número e 1 símbolo, e no mínimo 8 caracteres.");
        }
    }
}

