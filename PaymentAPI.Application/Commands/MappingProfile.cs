using AutoMapper;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Commands
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterSaleCommand, Sale>();

            CreateMap<UpdateSaleCommand, Sale>()
                .AfterMap((src, dest) => {
                    dest.Seller.SellerId = src.Id;
                    foreach (var item in dest.Items)
                    {
                        item.SaleId = src.Id;
                    }
                });

            CreateMap<ItemRequest, Item>();

            CreateMap<SellerRequest, Seller>();
        }
    }
}
