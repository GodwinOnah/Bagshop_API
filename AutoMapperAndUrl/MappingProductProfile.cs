using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.AutoMapperAndUrl;
using API.DTOs;
using AutoMapper;
using core;
using core.Controllers;
using core.Entities.DTOs;
using core.Entities.Identity;
using core.Entities.Oders;

namespace API.AutoMapper
{
    public class MappingProductProfile : Profile//this code removes the null property of product brand and types
    {
        public MappingProductProfile()
        {
            CreateMap<Products, ProductsShapedObject>()
            .ForMember(x=>x.productBrand,y=>y.MapFrom(z=>z.productBrand.Name))
            .ForMember(x=>x.productType,y=>y.MapFrom(z=>z.productType.Name))
            .ForMember(x=>x.prodPicture,y=>y.MapFrom<ProductPictureUrl>());

            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<BasketDTO, Basket>();
            CreateMap<BasketItemsDTO, BasketItems>();
             CreateMap<AddressDTO,ShippingAddress>();

              CreateMap<Order, OrderDTOFinal>()
              .ForMember(d=>d.delivery,o=>o.MapFrom(s=>s.delivery.delName))
              .ForMember(d=>d.delivery,o=>o.MapFrom(s=>s.delivery.delPrice));

               CreateMap<ItemOrdered, ItemOrderedDTO>()
               .ForMember(d=>d.id,o=>o.MapFrom(s=>s.itemOrdered.id))
               .ForMember(d=>d.prodName,o=>o.MapFrom(s=>s.itemOrdered.prodName))
               .ForMember(d=>d.prodPicture,o=>o.MapFrom(s=>s.itemOrdered.prodPicture))
               .ForMember(x=>x.prodPicture,y=>y.MapFrom<OrderPictureUrlResolver>());

               CreateMap<ProductDetails, Products>()
             
               .ForMember(d=>d.prodName,o=>o.MapFrom(s=>s.prodName))
                .ForMember(x=>x.prodPicture,y=>y.MapFrom<ProductDetailsPicture>())
                .ForMember(d=>d.prodDescription,o=>o.MapFrom(s=>s.prodDescription))
                 .ForMember(d=>d.prodPrice,o=>o.MapFrom(s=>s.prodPrice))
                  .ForMember(d=>d.productBrand,o=>o.MapFrom(s=>s.productBrand))
                   .ForMember(d=>d.productType,o=>o.MapFrom(s=>s.productType));
               
         }
    }
}