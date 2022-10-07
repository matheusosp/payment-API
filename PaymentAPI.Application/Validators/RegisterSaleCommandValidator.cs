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
                .NotEmpty();

            RuleFor(c => c.Seller).SetValidator(new SellerValidator());
            RuleForEach(c => c.Items).SetValidator(new ItemValidator());
        }
    }
}
