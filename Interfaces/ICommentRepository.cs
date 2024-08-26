using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(int CommentId);
        Task<Comment> CreatedAsync(Comment commentModel);
        Task<Comment> UpdatedAsync(int CommentId, Comment commentModel);
        Task<Comment> DeleteAsync(int CommentId);
    }
}