using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Order;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class OrderMappers
    {
        public static OrderDto ToOrderDto(this Order orderModel)
        {
            return new OrderDto
            {
                OrderId = orderModel.OrderId,
                OrderStatus = orderModel.OrderStatus,
                PaymentStatus = orderModel.PaymentStatus,
                TotalAmount = orderModel.TotalAmount
            };
        }

        public static Order ToorderModel(this OrderDto orderDto)
        {
            return new Order
            {
                OrderId = orderDto.OrderId,
                OrderStatus = orderDto.OrderStatus,
                PaymentStatus = orderDto.PaymentStatus,
                TotalAmount = orderDto.TotalAmount
            };
        }
    }
}