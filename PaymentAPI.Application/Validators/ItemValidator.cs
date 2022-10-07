using FluentValidation;
using PaymentAPI.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validators
{
    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("A propriedade {PropertyName} deve estar preenchida.")
                .Length(2, 20).WithMessage("{PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Price)
                .NotEmpty().WithMessage("A propriedade {PropertyName} deve estar preenchida.")
                .GreaterThanOrEqualTo(0).WithMessage("A propriedade {PropertyName} deve ser maior ou igual a 0.");

            RuleFor(c => c.Quantity)
                .NotEmpty().WithMessage("A propriedade {PropertyName} deve estar preenchida.")
                .GreaterThan(0).WithMessage("A propriedade {PropertyName} deve ser maior que 0."); ;
        }
    }
}
