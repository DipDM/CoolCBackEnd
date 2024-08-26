using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Order;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
         private readonly IOrderRepository _orderRepo;
        public OrderController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var order = await _orderRepo.GetAllAsync();
            return Ok(order);
        }

        [HttpGet("{OrderId:Guid}")]
        public async Task<IActionResult> GetById(Guid OrderId)
        {
            var order = await _orderRepo.GetByIdAsync(OrderId);

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateOrderRequestDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderModel = new Order
            {
                OrderStatus = orderDto.OrderStatus,
                PaymentStatus = orderDto.PaymentStatus,
                TotalAmount = orderDto.TotalAmount  
            };
            var createdOrder = await _orderRepo.CreateAsync(orderModel);
            return CreatedAtAction(nameof(GetById), new { OrderId = createdOrder.OrderId }, createdOrder);
        }

        [HttpDelete("{OrderId:Guid}")]
        public async Task<IActionResult> Delete(Guid OrderId)
        {
            var deletedOrder = await _orderRepo.DeleteAsync(OrderId);

            if (deletedOrder == null)
            {
                return NotFound();
            }
            return Ok(deletedOrder);
        }
    }
}