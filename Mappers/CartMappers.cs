using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Cart;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CoolCBackEnd.Mappers
{
    public static class CartMappers
    {
        public static CartDto ToCartDto(this Cart cartModel)
        {
            return new CartDto
            {
                CartId = cartModel.CartId,
            };
        }

        public static Cart TocartModel(this CartDto cartDto)
        {
            return new Cart
            {
                CartId = cartDto.CartId,
            };
        }
    }
}