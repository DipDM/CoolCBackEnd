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
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDBContext _context;
        public CommentController(ICommentRepository commentRepo, IProductRepository productRepo, ApplicationDBContext context, UserManager<User> userManager)
        {
            _commentRepo = commentRepo;
            _productRepo = productRepo;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get all comments from the repository
            var comments = await _commentRepo.GetAllAsync();

            // Create a list to hold the comment DTOs
            var commentDtos = new List<CommentDto>();

            foreach (var comment in comments)
            {
                // User should be included with the comment now
                var user = comment.User;  // Access User directly

                // If the user doesn't exist, you can choose to either skip or return an error
                if (user == null)
                {
                    continue; // Skip this comment or handle accordingly (e.g., return an error response)
                }

                // Convert each comment to a CommentDto and include the UserName
                var commentDto = new CommentDto
                {
                    CommentId = comment.CommentId,
                    UserId = comment.UserId,
                    CommentText = comment.CommentText,
                    Rating = comment.Rating,
                    UserName = user.UserName, // Get the UserName
                    ProductId = comment.ProductId
                };

                // Add the comment DTO to the list
                commentDtos.Add(commentDto);
            }

            return Ok(commentDtos);
        }

        [HttpGet("{CommentId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int CommentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepo.GetByIdAsync(CommentId);

            if (comment == null)
            {
                return NotFound();
            }

            // Convert the Comment to CommentDto
            var commentDto = comment.ToCommentDto();

            return Ok(commentDto);
        }

        [HttpPost("{productId:int}")]
        public async Task<IActionResult> Create(int productId, [FromBody] CreateCommentRequestDto commentDto)
        {
            // Validate UserId
            var user = await _context.Users
                                      .Where(u => u.Id == commentDto.UserId)
                                      .Select(u => new { u.Id, u.UserName })
                                      .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("Invalid UserId. User does not exist.");
            }

            // Create the Comment
            var comment = new Comment
            {
                ProductId = productId,
                UserId = user.Id,
                UserName = user.UserName,  // Assign UserName here
                CommentText = commentDto.CommentText,
                Rating = commentDto.Rating,
                // set other fields as needed
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