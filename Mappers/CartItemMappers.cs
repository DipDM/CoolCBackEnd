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
                CartItemId = cartItemModel.CartItemId,
                CartId = cartItemModel.CartId,
                ProductId = cartItemModel.ProductId,
                Quantity = cartItemModel.Quantity,
                Price = cartItemModel.Price,
                SizeId = cartItemModel.SizeId
            };
        }

        public static CartItem ToCartItemFromCreate(this CreateCartItemDto cartItemDto)
        {
            return new CartItem
            {
                CartId = cartItemDto.CartId,
                ProductId = cartItemDto.ProductId,
                Quantity = cartItemDto.Quantity,
                SizeId = cartItemDto.SizeId
            };
        }

        public static CartItem ToCartItemFromUpdate(this UpdateCartItemDto cartItemDto)
        {
            return new CartItem
            {
                Quantity = cartItemDto.Quantity,
                SizeId = cartItemDto.SizeId
            };
        }
    }
}