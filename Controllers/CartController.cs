using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Cart;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;
        private readonly ApplicationDBContext _context;
        public CartController(ICartRepository cartRepo, ApplicationDBContext context)
        {
            _cartRepo = cartRepo;
            _context = context;
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
        [HttpPost("check-or-create-cart")]
        public async Task<IActionResult> CheckOrCreateCart([FromBody] UserCartDto dto)
        {
            try
            {
                var userId = dto.UserId;

                // Step 1: Fetch the cart for the user by userId
                var cart = await _context.Carts
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                // Step 2: If no cart exists, create a new cart
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                        TotalAmount = 0 // Initialize total to 0 as no items yet
                    };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();  // Save to generate CartId for further use
                }

                // Step 3: Return the cart details (CartId, UserId, TotalAmount)
                return Ok(new
                {
                    Message = cart == null ? "New cart created" : "Cart already exists",
                    CartId = cart.CartId,
                    UserId = cart.UserId,
                    TotalAmount = cart.TotalAmount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
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