using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.CartItem;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemController(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetAllCartItems()
        {
            var cartItems = await _cartItemRepository.GetAllAsync();
            var cartItemDtos = cartItems.Select(CartItemMappers.ToCartItemDto).ToList();
            return Ok(cartItemDtos);
        }

        [HttpGet("{CartItemId}")]
        public async Task<ActionResult<CartItemDto>> GetCartItemById(int CartItemId)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(CartItemId);
            if (cartItem == null) return NotFound();

            return Ok(cartItem.ToCartItemDto());
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> CreateCartItem([FromBody] CreateCartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cartItem = CartItemMappers.ToCartItemFromCreate(cartItemDto);
            var createdCartItem = await _cartItemRepository.CreateAsync(cartItem);

            return CreatedAtAction(nameof(GetCartItemById), new { CartItemId = createdCartItem.CartItemId }, createdCartItem.ToCartItemDto());
        }

        [HttpPut("{CartItemId}")]
        public async Task<ActionResult<CartItemDto>> UpdateCartItem(int CartItemId, [FromBody] UpdateCartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCartItem = await _cartItemRepository.UpdateAsync(CartItemId, cartItemDto);
            if (updatedCartItem == null) return NotFound();

            return Ok(updatedCartItem.ToCartItemDto());
        }

        [HttpDelete("{CartItemId}")]
        public async Task<IActionResult> DeleteCartItem(int CartItemId)
        {
            var deletedCartItem = await _cartItemRepository.DeleteAsync(CartItemId);
            if (deletedCartItem == null) return NotFound();

            return Ok(deletedCartItem);
        }
    }
}