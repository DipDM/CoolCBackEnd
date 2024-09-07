using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Size;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SizeController : ControllerBase
    {
        private readonly ISizeRepository _sizeRepo;
        public SizeController(ISizeRepository sizeRepo)
        {
            _sizeRepo = sizeRepo;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var size=await _sizeRepo.GetAllAsync();
            return Ok(size);
        }

        [HttpGet("{SizeId:int}")]
        public async Task<IActionResult> GetSizeById(int SizeId)
        {
            var size  = await _sizeRepo.GetByIdAsync(SizeId);

            if(size == null)
            {
                return NotFound();
            }
            return Ok(size);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SizeCreateDto sizeCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var sizeModel = new Size
            {
                SizeName = sizeCreateDto.SizeName
            };

            var size = await _sizeRepo.CreateAsync(sizeModel);
            return CreatedAtAction(nameof(GetSizeById), new { SizeId = size.SizeId }, size);
        }

        [HttpPut("{SizeId:int}")]
        public async Task<IActionResult> Update(int SizeId, SizeUpdateDto sizeUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingSize = await _sizeRepo.GetByIdAsync(SizeId);
            if (existingSize == null)
            {
                return NotFound();
            }
            if(!string.IsNullOrEmpty(sizeUpdateDto.SizeName))
            {
                existingSize.SizeName = sizeUpdateDto.SizeName;
            }

            var updateSize = await _sizeRepo.UpdateAsync(SizeId,existingSize);
            return Ok(updateSize);
        }

        [HttpDelete("{SizeId:int}")]
        public async Task<IActionResult> Delete(int SizeId)
        {
            var deleteSize = await _sizeRepo.DeleteAsync(SizeId);

            if(deleteSize == null)
            {
                return NotFound();
            }

            return Ok (deleteSize);
        }
    }
}