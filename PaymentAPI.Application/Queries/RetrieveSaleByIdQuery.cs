using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentAPI.Application.Commands;
using PaymentAPI.Application.Validators;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Features.Enums;
using PaymentAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Queries
{
    public class RetrieveSaleByIdQuery : IRequest<Result<Sale>>
    {
        public long Id { get; set; }

        public RetrieveSaleByIdQuery(long id)
        {
            Id = id;
        }
    }
    public class RetrieveSaleByIdQueryHandler : IRequestHandler<RetrieveSaleByIdQuery, Result<Sale>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public RetrieveSaleByIdQueryHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public Task<Result<Sale>> Handle(RetrieveSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var sale = _saleRepository.GetById(request.Id);

            if (sale == null)
            {
                return Task.FromResult(Result.Failure<Sale>("Id Não encontrado"));
            }

            return Task.FromResult(Result.Success(_mapper.Map<Sale>(sale)));
        }
    }
}
