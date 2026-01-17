using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.CouponOrder;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class CouponOrderMappers
    {
        public static CouponOrderDto ToCouponOrderDto(this CouponOrder couponOrderModel)
        {
            return new CouponOrderDto
            {
                CouponOrderId = couponOrderModel.CouponOrderId,
                CouponId = couponOrderModel.CouponId,
                OrderId = couponOrderModel.OrderId,
                DiscountAmount = couponOrderModel.DiscountAmount,
            };
        }
        public static CouponOrder ToCouponOrderFromCreate(this CouponOrderCreateDto couponOrderCreateDto)
        {
            return new CouponOrder
            {
                CouponId = couponOrderCreateDto.CouponId,
                OrderId = couponOrderCreateDto.OrderId,
                DiscountAmount = couponOrderCreateDto.DiscountAmount
            };
        }
        public static CouponOrder ToCouponOrderFromUpdate(this CouponOrderUpdateDto couponOrderUpdateDto)
        {
            return new CouponOrder
            {
                CouponId = couponOrderUpdateDto.CouponId,
                OrderId = couponOrderUpdateDto.OrderId,
                DiscountAmount = couponOrderUpdateDto.DiscountAmount
            };
        }

    }
}