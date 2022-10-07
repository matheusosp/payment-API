using FluentValidation;
using PaymentAPI.Application.Commands;
using PaymentAPI.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validators
{
    public class SellerValidator : AbstractValidator<Seller>
    {
        public SellerValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("A propriedade {PropertyName} deve estar preenchida.")
                .Length(2, 20).WithMessage("{PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Email)
                .NotEmpty().EmailAddress().WithMessage("{PropertyName} deve conter um email válido")
                .MaximumLength(20).WithMessage("{PropertyName} deve conter no máximo {MaxLength} caracteres");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("{PropertyName} deve ser preenchida.");

            RuleFor(x => x.CPF)
                .NotEmpty().WithMessage("{PropertyName} deve ser preenchida.");
        }
    }
}
