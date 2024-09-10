using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.CouponUser;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class CouponUserMappers
{
    public static CouponUserDto ToCouponUserDto(this CouponUser couponUserModel)
    {
        return new CouponUserDto
        {
            CouponUserId = couponUserModel.CouponUserId.ToString(),
            UserId = couponUserModel.UserId,
            CouponId = couponUserModel.CouponId,
            RedeemedDate = couponUserModel.RedeemedDate,
            OrderId = couponUserModel.OrderId
        };
    }

    public static CouponUser ToCouponUserFromCreateDto(this CouponUserCreateDto couponUserCreateDto)
    {
        return new CouponUser
        {
            UserId = couponUserCreateDto.UserId,
            CouponId = couponUserCreateDto.CouponId,
            RedeemedDate = couponUserCreateDto.RedeemedDate,
            OrderId = couponUserCreateDto.OrderId
        };
    }

    public static CouponUser ToCouponUserFromUpdateDto(this CouponUserUpdateDto couponUserUpdateDto)
    {
        return new CouponUser
        {
            UserId = couponUserUpdateDto.UserId,
            CouponId = couponUserUpdateDto.CouponId,
            RedeemedDate = couponUserUpdateDto.RedeemedDate,
            OrderId = couponUserUpdateDto.OrderId
        };
    }
}

}