using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.CartItem;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepo;
        private readonly ApplicationDBContext _context;  // Inject ApplicationDBContext

        public CartItemController(ICartItemRepository cartItemRepository, ApplicationDBContext context, ICartRepository cartRepo)
        {
            _cartItemRepository = cartItemRepository;
            _context = context;
            _cartRepo = cartRepo;
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

            // Automatically fetch and set the price in the repository
            var createdCartItem = await _cartItemRepository.CreateAsync(cartItem);

            return CreatedAtAction(nameof(GetCartItemById), new { CartItemId = createdCartItem.CartItemId }, createdCartItem.ToCartItemDto());
        }
        [HttpPost("add-or-update")]
        public async Task<ActionResult<CartItemDto>> AddOrUpdateCartItem([FromBody] CreateCartItemDto cartItemDto)
        {
            try
            {
                // Step 1: Add or Update the CartItem
                var updatedCartItem = await _cartItemRepository.AddOrUpdateCartItemAsync(
                    cartItemDto.CartId,
                    cartItemDto.ProductId,
                    cartItemDto.Quantity
                );

                if (updatedCartItem == null)
                {
                    return BadRequest("Product not found.");
                }

                // Step 2: Update the Cart's TotalAmount by summing the prices of all items in the cart
                await _cartRepo.UpdateCartTotalAmountAsync(cartItemDto.CartId);

                // Step 3: Fetch the updated Cart and its items
                var updatedCart = await _cartRepo.GetCartWithItemsAsync(cartItemDto.CartId);

                if (updatedCart == null)
                {
                    return NotFound("Cart not found.");
                }

                // Step 4: Return the updated cart and its items as a DTO
                return Ok(new
                {
                    CartId = updatedCart.CartId,
                    TotalAmount = updatedCart.TotalAmount,
                    Items = updatedCart.CartItems.Select(ci => ci.ToCartItemDto())
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in add-or-update method: {ex.Message}");
                return StatusCode(500, new { Error = ex.Message });
            }
        }




        [HttpPut("{CartItemId}")]
        public async Task<ActionResult<CartItemDto>> UpdateCartItem(int CartItemId, [FromBody] UpdateCartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Automatically update the price based on the product price and quantity
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

        [HttpGet("test-product-price/{productId}")]
        public async Task<IActionResult> TestProductPrice(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(new { product.Price });
        }

    }
}
