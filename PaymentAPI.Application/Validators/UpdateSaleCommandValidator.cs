using FluentValidation;
using PaymentAPI.Application.Commands;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Features.Enums;
using PaymentAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validators
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator(SaleStatus dataBaseStatus)
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("A venda deve ter uma data");
            RuleFor(c => c.Status).IsInEnum().WithMessage("O Status não pode ser 0 ou vazio")
                .Must((c, status) => CanChangeStatus(dataBaseStatus, status)).WithMessage("O Status não pode ser alterado").When(c => c.Status > 0,ApplyConditionTo.CurrentValidator);

            RuleFor(c => c.Seller).NotEmpty().WithMessage("A venda deve ter um vendedor")
                .SetValidator(new SellerValidator());
            RuleFor(c => c.Items).NotEmpty().WithMessage("A venda tem que ter pelo menos 1 item.");
            RuleForEach(c => c.Items).NotEmpty().SetValidator(new ItemValidator());
        }

        public bool CanChangeStatus(SaleStatus databaseStatus, SaleStatus newStatus) 
        {
            if (databaseStatus == newStatus)
                return true;

            if (databaseStatus == SaleStatus.AwaitingPayment) { 
                if(newStatus == SaleStatus.Canceled || newStatus == SaleStatus.ApprovedPayment)
                    return true;
                return false;
            }
            if (databaseStatus == SaleStatus.ApprovedPayment)
            {
                if (newStatus == SaleStatus.Canceled || newStatus == SaleStatus.SentToCarrier)
                    return true;
                return false;
            }
            if (databaseStatus == SaleStatus.SentToCarrier)
            {
                if (newStatus == SaleStatus.Delivered)
                    return true;
                return false;
            }
            return false;
        }
    }
}
