using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Comment;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                CommentId = commentModel.CommentId,
                CommentText = commentModel.CommentText,
                Rating = commentModel.Rating,
                ProductId = commentModel.ProductId
            };
        }

        public static Comment ToCommentFromCreate(this CreateCommentRequestDto commentDto, int ProductId)
        {
            return new Comment
            {
                CommentText = commentDto.CommentText,
                Rating = commentDto.Rating ?? 0,
                ProductId = ProductId
            };
        }

        public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto)
        {
            return new Comment
            {
                CommentText = commentDto.CommentText,
                Rating = commentDto.Rating ?? 0
            };
        }
    }
}