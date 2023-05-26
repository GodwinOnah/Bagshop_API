using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.ErrorsHandlers;
using AutoMapper;
using core.Entities.DTOs;
using core.Entities.Identity;
using core.Entities.Oders;
using core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : ApiControllerBase
    {

      
        private readonly IAdminOrder _iAdminOrders;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public AdminController(IAdminOrder iAdminOrders, IMapper mapper)
        {
            _mapper = mapper;
            _iAdminOrders = iAdminOrders;
        }
        
        [HttpGet]
         public async Task<ActionResult<IReadOnlyList<AdminOrderDTO>>> GetAdinOrderForUser(){

            var adminOrders = await  _iAdminOrders.GetAdminOrderhsAsync(OrderStatus.PaymentReceived);   
            return Ok(_mapper.Map<IReadOnlyList<AdminOrderDTO>>(adminOrders));
    }
         [HttpGet("{id}")]
         public async Task<ActionResult<AdminOrderDTO>> GetAdminOrderForUser(int id){
           
            var adminOrders = await  _iAdminOrders.GetAdminOrdersByIdAsync(id);
            if( adminOrders == null ) return NotFound(new Responses(400));
            return _mapper.Map<AdminOrderDTO>(adminOrders);
    }

         [HttpDelete("{id}")]
        public async Task<bool> DeleteAdminOrder(int id)
        {
          await _iAdminOrders.DeletAdminOrder(id);
          
            return true;
        }
    }
}