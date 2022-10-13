using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentAPI.Application.Validators;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Features.Enums;
using PaymentAPI.Domain.Interfaces;


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
        private readonly IMapper _mapper;

        public RegisterSaleCommandHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Sale>> Handle(RegisterSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new RegisterSaleCommandValidator();

            var result = validator.Validate(request);
            var errors = new List<string>();
            var sale = _mapper.Map<Sale>(request);
            if (result.IsValid == false)
            {
                errors.AddRange(result.Errors.Select(p => p.ErrorMessage));
            }
            else 
            {
                
                sale.Status = SaleStatus.AwaitingPayment;
                sale = _saleRepository.Add(sale);

                await _unitOfWork.CommitAsync();
            }

            return Result.SuccessIf(result.IsValid,sale,string.Join('\n', errors));
        }
    }
}
