using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.CartItem;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class CartItemMappers
    {
        public static CartItemDto ToCartItemDto(this CartItem cartItemModel)
        {
            return new CartItemDto
            {
                CartId = cartItemModel.CartId,
                ProductId = cartItemModel.ProductId,
                Quantity = cartItemModel.Quantity
            };
        }

        public static CartItem ToCartItemFromCreate(this CreateCartItemDto cartItemDto)
        {
            return new CartItem
            {
                CartId = cartItemDto.CartId,
                ProductId = cartItemDto.ProductId,
                Quantity = cartItemDto.Quantity,
                CartItemId = cartItemDto.CartItemId
            };
        }

        public static CartItem ToCartItemFromUpdate(this UpdateCartItemDto cartItemDto)
        {
            return new CartItem
            {
                Quantity = cartItemDto.Quantity
            };
        }
    }
}