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
    public class RegisterSaleCommandMapping : Profile
    {
        public RegisterSaleCommandMapping()
        {
            CreateMap<RegisterSaleCommand, Sale>()
                .ForMember(m => m.Date, opts => opts.MapFrom(src => src.Date));

            CreateMap<ItemRequest, Item>()
                .ForMember(m => m.Quantity, opts => opts.MapFrom(src => src.Quantity))
                .ForMember(m => m.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(m => m.Price, opts => opts.MapFrom(src => src.Price));

            CreateMap<SellerRequest, Seller>()
                .ForMember(m => m.Email, opts => opts.MapFrom(src => src.Email))
                .ForMember(m => m.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(m => m.Phone, opts => opts.MapFrom(src => src.Phone));
        }
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
            if (result.IsValid == false)
            {
                errors.AddRange(result.Errors.Select(p => p.ErrorMessage));
            }

            var sale = _mapper.Map<Sale>(request);
            sale.Status = SaleStatus.AwaitingPayment;
            sale = _saleRepository.Add(sale);
            
            await _unitOfWork.CommitAsync();

            return Result.SuccessIf(result.IsValid,sale,string.Join('\n', errors));
        }
    }
}
