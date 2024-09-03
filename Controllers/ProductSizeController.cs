using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.ProductSize;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
[Route("api/[controller]")]
public class ProductSizeController : ControllerBase
{
    private readonly IProductSizeRepository _productSizeRepo;

    public ProductSizeController(IProductSizeRepository productSizeRepo)
    {
        _productSizeRepo = productSizeRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var productSize = await _productSizeRepo.GetAllAsync();
        return Ok(productSize);
    }

    [HttpGet("{ProductSizeId:int}")]
    public async Task<IActionResult> GetProductSizeById(int ProductSizeId)
    {
        var productSize = await _productSizeRepo.GetByIdAsync(ProductSizeId);

        if (productSize == null)
        {
            return NotFound();
        }
        return Ok(productSize);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductSizeDto productSizeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var productSizeModel = new ProductSize
        {
            ProductId = productSizeDto.ProductId,
            SizeId = productSizeDto.SizeId
        };

        var productSize = await _productSizeRepo.CreateAsync(productSizeModel);
        return CreatedAtAction(nameof(GetProductSizeById), new { ProductSizeId = productSize.ProductSizeId }, productSize);
    }

    [HttpPut("{ProductSizeId:int}")]
    public async Task<IActionResult> Update(int ProductSizeId, ProductSizeDto productSizeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingProductSize = await _productSizeRepo.GetByIdAsync(ProductSizeId);
        if (existingProductSize == null)
        {
            return NotFound();
        }

        existingProductSize.ProductId = productSizeDto.ProductId;
        existingProductSize.SizeId = productSizeDto.SizeId;

        var updateProductSize = await _productSizeRepo.UpdateAsync(ProductSizeId, existingProductSize);
        return Ok(updateProductSize);
    }

    [HttpDelete("{ProductSizeId:int}")]
    public async Task<IActionResult> Delete(int ProductSizeId)
    {
        var deleteProductSize = await _productSizeRepo.DeleteAsync(ProductSizeId);
        if (deleteProductSize == null)
        {
            return NotFound();
        }
        return Ok(deleteProductSize);
    }
} 
}