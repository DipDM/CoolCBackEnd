using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.OrderItem;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _OrderItemRepository;

        public OrderItemController(IOrderItemRepository OrderItemRepository)
        {
            _OrderItemRepository = OrderItemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetAllOrderItems()
        {
            var OrderItems = await _OrderItemRepository.GetAllAsync();
            return Ok(OrderItems.Select(ci => ci.ToOrderItemDto()));
        }

        [HttpGet("{OrderItemId}")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItemById(int OrderItemId)
        {
            var OrderItem = await _OrderItemRepository.GetByIdAsync(OrderItemId);
            if (OrderItem == null) return NotFound();

            return Ok(OrderItem.ToOrderItemDto());
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItem([FromForm] CreateOrderItemRequest OrderItemDto)
        {
            var OrderItem = OrderItemDto.ToOrderItemFromCreate();
            var createdOrderItem = await _OrderItemRepository.CreateAsync(OrderItem);

            return CreatedAtAction(nameof(GetOrderItemById), new { OrderItemId = createdOrderItem.OrderItemId }, createdOrderItem.ToOrderItemDto());
        }
        

        [HttpPut("{OrderItemId}")]
        public async Task<ActionResult<OrderItemDto>> UpdateOrderItem(int OrderItemId, UpdateOrderItemRequestDto OrderItemDto)
        {
            var updatedOrderItem = await _OrderItemRepository.UpdateAsync(OrderItemId, OrderItemDto);
            if (updatedOrderItem == null) return NotFound();

            return Ok(updatedOrderItem.ToOrderItemDto());
        }

        [HttpDelete("{OrderItemId}")]
        public async Task<IActionResult> DeleteOrderItem(int OrderItemId)
        {
            var deletedOrderItem = await _OrderItemRepository.DeleteAsync(OrderItemId);
            if (deletedOrderItem == null) return NotFound();
        

            return NoContent();
        }
    }
}