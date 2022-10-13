using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentAPI.Application.Validators;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Features.Enums;
using PaymentAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Commands
{
    public class UpdateSaleCommand : IRequest<Result<Sale>>
    {
        [JsonIgnore]
        public long Id { get; set; }
        public SellerRequest Seller { get; set; }
        public DateTime Date { get; set; }
        public SaleStatus Status { get; set; }
        public ICollection<ItemRequest> Items { get; set; }
    }

    public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, Result<Sale>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSaleCommandHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Sale>> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var databaseSale = _saleRepository.GetById(request.Id);
            var validator = new UpdateSaleCommandValidator(databaseSale.Status);
            var result = validator.Validate(request);
            
            if (result.IsValid == false)
            {
                return Result.SuccessIf(false,new Sale(), string.Join('\n', result.Errors.Select(p => p.ErrorMessage)));
            }

            var sale = _mapper.Map<Sale>(request);

            _saleRepository.UpdateSaleById(sale, databaseSale);

            await _unitOfWork.CommitAsync();
            return Result.SuccessIf(result.IsValid, sale, "");
            
            
        }
    }
}
