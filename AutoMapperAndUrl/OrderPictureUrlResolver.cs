using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using core.Entities.Oders;
using Microsoft.Extensions.Configuration;

namespace API.AutoMapperAndUrl
{
    public class OrderPictureUrlResolver: IValueResolver<ItemOrdered, ItemOrderedDTO, string>
    {
        private readonly IConfiguration _configuration;
        public OrderPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(ItemOrdered source, ItemOrderedDTO destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.productOrdered.prodPicture))
            {

                return _configuration["ApiUrl"]+source.productOrdered.prodPicture;
            }

            return null;
        }
    }
        
    }
