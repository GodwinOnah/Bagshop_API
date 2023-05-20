using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.ErrorsHandlers;
using AutoMapper;
using core.Entities.Identity;
using core.Entities.Oders;
using core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : ApiControllerBase
    {

      
        private readonly IOrders _iOrders;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public AdminController(IOrders iOrders, IMapper mapper, UserManager<User> userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _iOrders = iOrders;
        }
        
        [HttpGet]
         public async Task<ActionResult<IReadOnlyList<OrderDTOFinal>>> GetPaidOrderForUser(){

            var orders = await  _iOrders.GetPaidOrdersAsync(OrederStatus.Pending);   
            return Ok(_mapper.Map<IReadOnlyList<OrderDTOFinal>>(orders));
    }
         [HttpGet("{id}")]
         public async Task<ActionResult<OrderDTOFinal>> GetPaidOrderForUser(int id){
           
            var order = await  _iOrders.GetPaidOrdersByIdAsync(id);
            if( order == null ) return NotFound(new Responses(400));
            return _mapper.Map<OrderDTOFinal>(order);
    }
    }
}