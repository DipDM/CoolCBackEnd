using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.CouponUser;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
   [ApiController]
[Route("api/[controller]")]
public class CouponUserController : ControllerBase
{
    private readonly ICouponUserRepository _couponUserRepository;

    public CouponUserController(ICouponUserRepository couponUserRepository)
    {
        _couponUserRepository = couponUserRepository;
    }

    // GET: api/couponuser
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var couponUsers = await _couponUserRepository.GetAllAsync();
        return Ok(couponUsers.Select(cu => cu.ToCouponUserDto()));
    }

    // GET: api/couponuser/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var couponUser = await _couponUserRepository.GetByIdAsync(id);
        if (couponUser == null)
        {
            return NotFound();
        }

        return Ok(couponUser.ToCouponUserDto());
    }

    // POST: api/couponuser
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CouponUserCreateDto couponUserCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var couponUser = couponUserCreateDto.ToCouponUserFromCreateDto();
        var createdCouponUser = await _couponUserRepository.CreateAsync(couponUser);

        return CreatedAtAction(nameof(GetById), new { id = createdCouponUser.CouponUserId }, createdCouponUser.ToCouponUserDto());
    }

    // PUT: api/couponuser/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CouponUserUpdateDto couponUserUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingCouponUser = await _couponUserRepository.GetByIdAsync(id);
        if (existingCouponUser == null)
        {
            return NotFound();
        }

        var updatedCouponUser = couponUserUpdateDto.ToCouponUserFromUpdateDto();
        var result = await _couponUserRepository.UpdateAsync(id, updatedCouponUser);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result.ToCouponUserDto());
    }

    // DELETE: api/couponuser/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var couponUser = await _couponUserRepository.DeleteAsync(id);
        if (couponUser == null)
        {
            return NotFound();
        }

        return Ok(couponUser.ToCouponUserDto());
    }
}

}