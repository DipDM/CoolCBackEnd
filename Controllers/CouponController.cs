using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Coupon;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepo;

        public CouponController(ICouponRepository couponRepo)
        {
            _couponRepo = couponRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var coupon = await _couponRepo.GetAllAsync();
            return Ok(coupon);
        }

        [HttpGet("{CouponId:int}")]
        public async Task<IActionResult> GetById(int CouponId)
        {
            var coupon = await _couponRepo.GetByIdAsync(CouponId);

            if (coupon == null)
            {
                return NotFound();
            }
            return Ok(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> Create( CouponCreateDto couponDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var couponModel = new Coupon
            {
                Code = couponDto.Code,
                DiscountType = couponDto.DiscountType,
                DiscountValue = couponDto.DiscountValue,
                MaxDiscountAmount = couponDto.MaxDiscountAmount,
                MinOrderAmount = couponDto.MinOrderAmount,
                StartDate = couponDto.StartDate,
                EndDate = couponDto.EndDate,
                IsActive = couponDto.IsActive,
                UsageLimit = couponDto.UsageLimit,
            };

            var createdCoupon = await _couponRepo.CreatedAsync(couponModel);
            return CreatedAtAction(nameof(GetById), new { CouponId = createdCoupon.CouponId }, createdCoupon);
        }

        [HttpPut("{CouponId:int}")]
        public async Task<IActionResult> Update(int CouponId,  CouponUpdateDto couponUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCoupon = await _couponRepo.GetByIdAsync(CouponId);
            if (existingCoupon == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(couponUpdateDto.Code))
            {
                existingCoupon.Code = couponUpdateDto.Code;
            }
            if (!string.IsNullOrEmpty(couponUpdateDto.DiscountType))
            {
                existingCoupon.DiscountType = couponUpdateDto.DiscountType;
            }

            existingCoupon.DiscountValue = couponUpdateDto.DiscountValue;
            existingCoupon.MaxDiscountAmount = couponUpdateDto.MaxDiscountAmount;
            existingCoupon.MinOrderAmount = couponUpdateDto.MinOrderAmount;
            existingCoupon.StartDate = couponUpdateDto.StartDate;
            existingCoupon.EndDate = couponUpdateDto.EndDate;
            existingCoupon.IsActive = couponUpdateDto.IsActive;
            existingCoupon.UsageLimit = couponUpdateDto.UsageLimit;
            existingCoupon.UsageCount = couponUpdateDto.UsageCount;

            var updatedCoupon = await _couponRepo.UpdatedAsync(CouponId, existingCoupon);
            return Ok(updatedCoupon);
        }


        
        [HttpDelete("{CouponId:int}")]
        public async Task<IActionResult> Delete(int CouponId)
        {
            var deletedCoupon = await _couponRepo.DeleteAsync(CouponId);

            if (deletedCoupon == null)
            {
                return NotFound();
            }
            return Ok(deletedCoupon);
        }
    }
}