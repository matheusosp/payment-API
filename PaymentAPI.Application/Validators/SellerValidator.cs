using FluentValidation;
using PaymentAPI.Application.Commands;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validators
{
    public class SellerValidator : AbstractValidator<SellerRequest>
    {
        public SellerValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("A propriedade {PropertyName} do vendedor deve estar preenchida.")
                .Length(2, 20).WithMessage("{PropertyName} do vendedor deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("A propriedade {PropertyName} do vendedor deve estar preenchida.")
                .EmailAddress().WithMessage("{PropertyName} do vendedor deve conter um email válido");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("{PropertyName} do vendedor deve ser preenchido.");

            RuleFor(x => x.CPF)
                .NotEmpty().WithMessage("{PropertyName} do vendedor deve ser preenchido.");
        }
    }
}
