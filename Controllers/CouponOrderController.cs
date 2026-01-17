using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.CouponOrder;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponOrderController : ControllerBase
    {
        private readonly ICouponOrderRepository _couponOrderRepository;

    public CouponOrderController(ICouponOrderRepository couponOrderRepository)
    {
        _couponOrderRepository = couponOrderRepository;
    }

    // GET: api/couponorder
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var couponOrders = await _couponOrderRepository.GetAllAsync();
        return Ok(couponOrders.Select(c => c.ToCouponOrderDto()));
    }

    // GET: api/couponorder/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var couponOrder = await _couponOrderRepository.GetByIdAsync(id);
        if (couponOrder == null)
        {
            return NotFound();
        }

        return Ok(couponOrder.ToCouponOrderDto());
    }

    // POST: api/couponorder
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CouponOrderCreateDto couponOrderCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var couponOrder = couponOrderCreateDto.ToCouponOrderFromCreate();
        var createdCouponOrder = await _couponOrderRepository.CreatedAsync(couponOrder);

        return CreatedAtAction(nameof(GetById), new { id = createdCouponOrder.CouponOrderId }, createdCouponOrder.ToCouponOrderDto());
    }

    // PUT: api/couponorder/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CouponOrderUpdateDto couponOrderUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingCouponOrder = await _couponOrderRepository.GetByIdAsync(id);
        if (existingCouponOrder == null)
        {
            return NotFound();
        }

        var updatedCouponOrder = couponOrderUpdateDto.ToCouponOrderFromUpdate();
        var result = await _couponOrderRepository.UpdatedAsync(id, updatedCouponOrder);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result.ToCouponOrderDto());
    }

    // DELETE: api/couponorder/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var couponOrder = await _couponOrderRepository.DeleteAsync(id);
        if (couponOrder == null)
        {
            return NotFound();
        }

        return Ok(couponOrder.ToCouponOrderDto());
    }
    }
}