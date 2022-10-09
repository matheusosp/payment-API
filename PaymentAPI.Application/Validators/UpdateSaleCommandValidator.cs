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
                    .NotEmpty();
            RuleFor(c => c.Status).NotNull().Must((c, status) => CanChangeStatus(dataBaseStatus, status)).WithMessage("O Status não pode ser alterado");
            RuleFor(c => c.Seller).SetValidator(new SellerValidator());
            RuleForEach(c => c.Items).SetValidator(new ItemValidator());
        }

        public bool CanChangeStatus(SaleStatus databaseStatus, SaleStatus newStatus) 
        {
            if (databaseStatus == SaleStatus.AwaitingPayment) { 
                if(newStatus == SaleStatus.Canceled || newStatus == SaleStatus.ApprovedPayment || newStatus == SaleStatus.AwaitingPayment)
                    return true;
                return false;
            }
            if (databaseStatus == SaleStatus.ApprovedPayment)
            {
                if (newStatus == SaleStatus.Canceled || newStatus == SaleStatus.SentToCarrier || newStatus == SaleStatus.ApprovedPayment)
                    return true;
                return false;
            }
            if (databaseStatus == SaleStatus.SentToCarrier)
            {
                if (newStatus == SaleStatus.Delivered || databaseStatus == SaleStatus.SentToCarrier)
                    return true;
                return false;
            }
            return true;
        }
    }
}
