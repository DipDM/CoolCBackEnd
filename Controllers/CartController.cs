using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Cart cartModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdCart = await _cartRepo.CreateAsync(cartModel);
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