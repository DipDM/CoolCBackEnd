using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.OrderItem;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class OrderItemMappers
    {
        public static OrderItemDto ToOrderItemDto(this OrderItem orderItemModel)
        {
            return new OrderItemDto
            {
                OrderId = orderItemModel.OrderId,
                ProductId = orderItemModel.ProductId,
                Quantity = orderItemModel.Quantity,
                Price = orderItemModel.Price
            };
        }

        public static OrderItem ToOrderItemFromCreate(this CreateOrderItemRequest orderItemDto)
        {
            return new OrderItem
            {
                OrderId = orderItemDto.OrderId,
                ProductId = orderItemDto.ProductId,
                Quantity = orderItemDto.Quantity,
                Price = orderItemDto.Price,
            };
        }

        public static OrderItem ToOrderItemFromUpdate(this UpdateOrderItemRequestDto orderItemDto)
        {
            return new OrderItem
            {
                Quantity = orderItemDto.Quantity,
                Price = orderItemDto.Price
            };
        }
    }
}