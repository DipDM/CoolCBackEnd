using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Comment> CreatedAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment> DeleteAsync(int CommentId)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.CommentId == CommentId);
            if(commentModel == null)
            {
                return null;
            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int CommentId)
        {
            return await _context.Comments.FirstOrDefaultAsync(x => x.CommentId == CommentId);
        }

        public async Task<Comment> UpdatedAsync(int CommentId, Comment commentModel)
        {
            var existingComment = await _context.Comments.FirstAsync();
            if(existingComment == null)
            {
                return null;
            }
            existingComment.CommentText = commentModel.CommentText;
            existingComment.Rating = commentModel.Rating;

            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}