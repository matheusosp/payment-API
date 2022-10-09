using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Features.Enums;
using PaymentAPI.Infra.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Commands
{
    public class RegisterSaleCommand: IRequest<Result<Sale>>
    {
        public SellerRequest Seller { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ItemRequest> Items { get; set; }
    }
    public class RegisterSaleCommandHandler:IRequestHandler<RegisterSaleCommand, Result<Sale>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterSaleCommandHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork)
        {
            _saleRepository = saleRepository;
            _unitOfWork = unitOfWork;   
        }

        public async Task<Result<Sale>> Handle(RegisterSaleCommand request, CancellationToken cancellationToken)
        {
            Sale sale = new Sale();
            sale.Status = SaleStatus.AwaitingPayment;
            sale.Date = request.Date;

            _saleRepository.Add(sale);

            await _unitOfWork.CommitAsync();

            return Result.Success(sale);
        }
    }
}
