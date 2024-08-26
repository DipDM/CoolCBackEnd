using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using CoolCBackEnd.Dtos.Comment;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IProductRepository _productRepo;

        public CommentController(ICommentRepository commentRepo, IProductRepository productRepo)
        {
            _commentRepo = commentRepo;
            _productRepo = productRepo;
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
            var comment = await _commentRepo.GetByIdAsync(CommentId);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{productId:int}")]
        public async Task<IActionResult> Create([FromRoute] int productId, [FromForm] CreateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _productRepo.ProductExists(productId))
            {
                return NotFound("The Model Does not Exisst U ");
            }

            var commentModel = commentDto.ToCommentFromCreate(productId);
            await _commentRepo.CreatedAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { CommentId = commentModel.CommentId }, commentModel.ToCommentDto());
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

            if(!string.IsNullOrEmpty(commentupdateDto.CommentText))
            {
                existingComment.CommentText = commentupdateDto.CommentText;
            }

            if(commentupdateDto.Rating.HasValue)
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