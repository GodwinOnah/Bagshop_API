using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using AutoMapper;
using core.Entities.Oders;
using core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using API.DTOs;
using core.Entities.Oders;
using API.ErrorsHandlers;
using API.Helper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using API.ApiExtensions;

namespace API.Controllers
{
    // [Authorize]
    public class OrderController : ApiControllerBase
    {
        private readonly IOrders _iOrders;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public OrderController(IOrders iOrders, IMapper mapper,UserManager<User> userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _iOrders = iOrders;
        }

    [HttpPost]
         public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO){
            var email = User.getEmailfromPrincipleClaims();
            var address = _mapper.Map<AddressDTO, ShippingAddress>(orderDTO.shippingAddress);
      //   Console.WriteLine("\n\n\n\n\n\n\n\n"+address.street+"\n\n\n\n\n\n\n\n");
            var order = await _iOrders.CreateOrdersAsync(email, orderDTO.basketId,
            orderDTO.deliveryId,address); 
            // Console.WriteLine("\n\n\n\n\n\n\n\n"+order.orderStatus+"\n\n\n\n\n\n\n\n");
            return Ok(order);
         }

    [HttpPut]
        public async Task<ActionResult<OrderDTOFinal>> UpdateOrder(OrderConfirmDetails details){
               var order = await  _iOrders.UpdateOrdersByIdAsync(details);
               
               return Ok(order);
        }

    // [Cashing(600)]
    [HttpGet]
         public async Task<ActionResult<IReadOnlyList<OrderDTOFinal>>> GetOrderForUser(){

            var email = HttpContext.User.getEmailfromPrincipleClaims();
            var orders = await  _iOrders.GetOrdersAsync(email);  

            return Ok(_mapper.Map<IReadOnlyList<OrderDTOFinal>>(orders));
    }

    // [Cashing(600)]
    [HttpGet("{id}")]
         public async Task<ActionResult<OrderDTOFinal>> GetOrderForUser(int id){
            var email = HttpContext.User.getEmailfromPrincipleClaims();
            var order = await  _iOrders.GetOrdersByIdAsync(id,email);
            if( order == null ) return NotFound(new Responses(400));
            return _mapper.Map<OrderDTOFinal>(order);
    }

   

    // [Cashing(600)]
    [HttpGet("delivery")]
         public async Task<ActionResult<IReadOnlyList<Delivery>>> GetDelivery(){
            var delivery = await _iOrders.GetDeliverysAsync();
            return Ok(delivery);
         }
}
}