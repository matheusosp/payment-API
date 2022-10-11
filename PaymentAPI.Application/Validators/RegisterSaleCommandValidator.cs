using FluentValidation;
using PaymentAPI.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validators
{
    public class RegisterSaleCommandValidator : AbstractValidator<RegisterSaleCommand>
    {
        public RegisterSaleCommandValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("A venda deve ter uma data");

            RuleFor(c => c.Seller).NotEmpty().WithMessage("A venda deve ter um vendedor")
                .SetValidator(new SellerValidator());
            RuleFor(c => c.Items).NotEmpty().WithMessage("A venda tem que ter pelo menos 1 item.");
            RuleForEach(c => c.Items).NotEmpty().SetValidator(new ItemValidator());
        }
    }
}
