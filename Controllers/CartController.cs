using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Cart;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;
        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cart = await _cartRepo.GetAllAsync();
            return Ok(cart);
        }

        [HttpGet("{CartId:int}")]
        public async Task<IActionResult> GetById(int CartId)
        {
            var cart = await _cartRepo.GetByIdAsync(CartId);

            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetCartByUserId(Guid userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }


        [HttpPut("{CartId:int}")]
        public async Task<IActionResult> Update(int CartId, [FromBody] UpdateCartRequestDto cartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cartToUpdate = await _cartRepo.GetByIdAsync(CartId);
            if (cartToUpdate == null)
            {
                return NotFound();
            }

            // Update cart fields here
            cartToUpdate.TotalAmount = cartDto.TotalAmount;  // Updating TotalAmount

            var updatedCart = await _cartRepo.UpdateAsync(CartId, cartToUpdate);

            if (updatedCart == null)
            {
                return BadRequest("Unable to update cart.");
            }

            return Ok(updatedCart);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCartRequestDto cartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cart = new Cart
            {
                UserId = cartDto.UserId,
                TotalAmount = 0
            };

            var createdCart = await _cartRepo.CreateAsync(cart);
            return CreatedAtAction(nameof(GetById), new { CartId = createdCart.CartId }, createdCart);
        }

        [HttpDelete("{CartId:int}")]
        public async Task<IActionResult> Delete(int CartId)
        {
            var deletedCart = await _cartRepo.DeleteAsync(CartId);

            if (deletedCart == null)
            {
                return NotFound();
            }
            return Ok(deletedCart);
        }

    }
}