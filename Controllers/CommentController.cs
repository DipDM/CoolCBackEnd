using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Comment;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IProductRepository _productRepo;

        private readonly ApplicationDBContext _context;
        public CommentController(ICommentRepository commentRepo, IProductRepository productRepo, ApplicationDBContext context)
        {
            _commentRepo = commentRepo;
            _productRepo = productRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(c => c.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet("{CommentId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int CommentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _context.Comments
        .Where(c => c.CommentId == CommentId)
        .Select(c => new CommentDto
        {
            CommentId = c.CommentId,
            UserId = c.UserId,
            CommentText = c.CommentText,
            Rating = c.Rating // Now this will be of type int?
        })
        .FirstOrDefaultAsync();

            Console.WriteLine($"Rating Value: {comment?.Rating}");

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpPost("{productId:int}")]
        public async Task<IActionResult> Create(int productId,[FromBody] CreateCommentRequestDto commentDto)
        {
            // Validate UserId
            var userExists = await _context.Users.AnyAsync(u => u.Id == commentDto.UserId);
            if (!userExists)
            {
                return BadRequest("Invalid UserId. User does not exist.");
            }

            // Create the Comment
            var comment = new Comment
            {
                ProductId = productId,
                UserId = commentDto.UserId,
                CommentText = commentDto.CommentText,
                Rating = commentDto.Rating,
                // set other fields
            };

            await _commentRepo.CreatedAsync(comment);
            return Ok("Comment created successfully");
        }

        [HttpPut]
        [Route("{CommentId:int}")]
        public async Task<IActionResult> Update([FromRoute] int CommentId, [FromForm] UpdateCommentRequestDto commentupdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingComment = await _commentRepo.GetByIdAsync(CommentId);

            if (existingComment == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(commentupdateDto.CommentText))
            {
                existingComment.CommentText = commentupdateDto.CommentText;
            }

            if (commentupdateDto.Rating.HasValue)
            {
                existingComment.Rating = commentupdateDto.Rating.Value;
            }

            // Perform the update
            var updatedComment = await _commentRepo.UpdatedAsync(CommentId, existingComment);

            // Check if the update was successful
            if (updatedComment == null)
            {
                return NotFound("Update failed. Comment not found.");
            }

            // Return the updated comment
            return Ok(updatedComment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{commentid:int}")]
        public async Task<IActionResult> Delete([FromRoute] int commentid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentModel = await _commentRepo.DeleteAsync(commentid);
            if (commentModel == null)
            {
                return NotFound("Comment Not Found");
            }
            return Ok(commentModel);
        }
    }
}