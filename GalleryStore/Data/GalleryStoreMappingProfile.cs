using AutoMapper;
using GalleryStore.Data.Entities;
using GalleryStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryStore.Data
{
    public class GalleryStoreMappingProfile : Profile
    {
        public GalleryStoreMappingProfile()
        {
            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderId, ex => ex.MapFrom(o => o.Id))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap();

            CreateMap<Product, ProductViewModel>()
                .ForMember(p => p.ProductId, ex => ex.MapFrom(p => p.Id))
                .ReverseMap();
        }
    }
}
