using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Coupon;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class CouponMappers
    {
        public static CouponDto ToCouponDto(this Coupon couponModel)
        {
            return new CouponDto
            {
                CouponId = couponModel.CouponId,
                Code = couponModel.Code,
                DiscountType = couponModel.DiscountType,
                DiscountValue = couponModel.DiscountValue,   // Added this line
                MaxDiscountAmount = couponModel.MaxDiscountAmount,
                MinOrderAmount = couponModel.MinOrderAmount,
                StartDate = couponModel.StartDate,
                EndDate = couponModel.EndDate,
                IsActive = couponModel.IsActive,
                UsageLimit = couponModel.UsageLimit,
                UsageCount = couponModel.UsageCount,
                CreatedDate = couponModel.CreatedDate,
                UpdatedDate = couponModel.UpdatedDate
            };
        }

        public static Coupon ToCouponFromCreateDto(this CouponCreateDto couponCreateDto)
        {
            return new Coupon
            {
                Code = couponCreateDto.Code,
                DiscountType = couponCreateDto.DiscountType,
                DiscountValue = couponCreateDto.DiscountValue,  // Added this line
                MaxDiscountAmount = couponCreateDto.MaxDiscountAmount,
                MinOrderAmount = couponCreateDto.MinOrderAmount,
                StartDate = couponCreateDto.StartDate,
                EndDate = couponCreateDto.EndDate,
                IsActive = couponCreateDto.IsActive,
                UsageLimit = couponCreateDto.UsageLimit,
                UsageCount = 0,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
        }

        public static Coupon ToCouponFromUpdateDto(this CouponUpdateDto couponUpdateDto)
        {
            return new Coupon
            {
                Code = couponUpdateDto.Code,
                DiscountType = couponUpdateDto.DiscountType,
                DiscountValue = couponUpdateDto.DiscountValue,   // Added this line
                MaxDiscountAmount = couponUpdateDto.MaxDiscountAmount,
                MinOrderAmount = couponUpdateDto.MinOrderAmount,
                StartDate = couponUpdateDto.StartDate,
                EndDate = couponUpdateDto.EndDate,
                IsActive = couponUpdateDto.IsActive,
                UsageLimit = couponUpdateDto.UsageLimit,
                UsageCount= couponUpdateDto.UsageCount,
                UpdatedDate = DateTime.Now
            };
        }
    }
}