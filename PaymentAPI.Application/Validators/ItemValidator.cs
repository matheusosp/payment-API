using FluentValidation;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validators
{
    public class ItemValidator : AbstractValidator<ItemRequest>
    {
        public ItemValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Propriedade {PropertyName} do item deve estar preenchida.")
                .Length(2, 20).WithMessage("{PropertyName} do item deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Price)
                .NotEmpty().WithMessage("Propriedade {PropertyName} do item deve estar preenchida.")
                .GreaterThanOrEqualTo(0).WithMessage("Propriedade {PropertyName} do item deve ser maior ou igual a 0.");

            RuleFor(c => c.Quantity)
                .GreaterThan(0).WithMessage("Propriedade {PropertyName} do item deve estar preenchida e ser maior que 0."); ;
        }
    }
}
