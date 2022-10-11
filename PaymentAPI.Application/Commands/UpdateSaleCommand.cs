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
    public class UpdateSaleCommandMapping : Profile
    {
        public UpdateSaleCommandMapping()
        {
            CreateMap<UpdateSaleCommand, Sale>()
                .ForMember(m => m.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(m => m.Status, opts => opts.MapFrom(src => src.Status))
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

            var errors = new List<string>();
            var sale = _mapper.Map<Sale>(request);
            if (result.IsValid == false)
            {
                errors.AddRange(result.Errors.Select(p => p.ErrorMessage));
            }
            else
            {
                sale.Seller.SellerId = databaseSale.Seller.SellerId;
                foreach (var item in sale.Items)
                {
                    item.SaleId = databaseSale.Id;
                }
                _saleRepository.UpdateSaleById(sale);

                await _unitOfWork.CommitAsync();
            }
            
            return Result.SuccessIf(result.IsValid, sale, string.Join('\n', errors));
        }
    }
}
