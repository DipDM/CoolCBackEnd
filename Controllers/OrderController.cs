using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Order;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly ICartRepository _cartRepo;
        private readonly ApplicationDBContext _context;

        public OrderController(IOrderRepository orderRepo, IOrderItemRepository orderItemRepo, ICartRepository cartRepo, ApplicationDBContext context)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _cartRepo = cartRepo;
            _context = context;
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
        public async Task<IActionResult> Create([FromBody] CreateOrderRequestDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == orderDto.UserId);
            if (!userExists)
            {
                return BadRequest("User does not exist.");
            }

            var orderModel = new Order
            {
                OrderStatus = orderDto.OrderStatus,
                PaymentStatus = orderDto.PaymentStatus,
                TotalAmount = orderDto.TotalAmount,
                UserId = orderDto.UserId
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

        [HttpPost("CreateOrderFromCart")]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] CreateOrderRequestDto orderDto)
        {
            try
            {
                var order = await _orderRepo.CreateOrderFromCartAsync(orderDto.CartId, orderDto.UserId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}