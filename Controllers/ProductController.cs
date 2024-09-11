using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Product;
using CoolCBackEnd.Dtos.ProductImage;
using CoolCBackEnd.Dtos.ProductSize;
using CoolCBackEnd.Helpers;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CoolCBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IProductRepository _productRepo;
        public ProductController(ApplicationDBContext context, IProductRepository productRepo)
        {
            _context = context;
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var products = await _productRepo.GetAllAsync(query);
            // Map products to ProductDto including ProductImages
            var productDto = products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                BrandId = p.BrandId,
                CategoryId = p.CategoryId,
                ProductImages = p.ProductImages.Select(pi => new ProductImageDto
                {
                    ProductImageId = pi.ProductImageId,
                    ImagePath = pi.ImagePath
                }).ToList(),
                ProductSizes = p.ProductSizes.Select(f => new ProductSizeDto
                {
                    ProductSizeId = f.ProductSizeId,
                    SizeId = f.SizeId
                }).ToList()
            }).ToList();
            var totalItems = await _productRepo.CountAsync(query);
            var totalPages = (int)Math.Ceiling((decimal)totalItems / query.PageSize);

            var response = new
            {
                Items = productDto,
                totalitems = totalItems,
                TotalPages = totalPages
            };
            return Ok(response);
        }
        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch the product including related ProductImages, ProductSizes, Brand, and Category
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSizes)
                .Include(p => p.Brand)       // Include the Brand entity
                .Include(p => p.Category)    // Include the Category entity
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            // Map the product to ProductDto including ProductImages, ProductSizes, Brand, and Category
            var productDto = new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                BrandId = product.BrandId,          // Ensure BrandId is populated
                CategoryId = product.CategoryId,    // Ensure CategoryId is populated
                ProductImages = product.ProductImages?.Select(pi => new ProductImageDto
                {
                    ProductImageId = pi.ProductImageId,
                    ImagePath = pi.ImagePath
                }).ToList() ?? new List<ProductImageDto>(),

                ProductSizes = product.ProductSizes?.Select(ps => new ProductSizeDto
                {
                    ProductSizeId = ps.ProductSizeId,
                    SizeId = ps.SizeId
                }).ToList() ?? new List<ProductSizeDto>(),
            };

            return Ok(productDto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequestDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productModel = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                BrandId = productDto.BrandId
            };
            // Example for debugging
            Console.WriteLine($"BrandId: {productDto.BrandId}");

            await _productRepo.CreatedAsync(productModel);
            return CreatedAtAction(nameof(GetById), new { ProductId = productModel.ProductId }, productModel.ToProductDto());
        }

        [HttpPut("{ProductId:int}")]
        public async Task<IActionResult> Update([FromRoute] int ProductId, [FromForm] UpdateProductRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = await _productRepo.GetByIdAsync(ProductId);
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(existingProduct.Name))
            {
                existingProduct.Name = updateDto.Name;
            }
            if (!string.IsNullOrEmpty(existingProduct.Description))
            {
                existingProduct.Description = updateDto.Description;
            }
            if (updateDto.Price.HasValue)
            {
                existingProduct.Price = updateDto.Price.Value;
            }
            if (updateDto.CategoryId.HasValue)
            {
                existingProduct.CategoryId = updateDto.CategoryId.Value;
            }
            if (updateDto.BrandId.HasValue)
            {
                existingProduct.BrandId = updateDto.BrandId.Value;
            }

            await _productRepo.UpdatedAsync(ProductId, updateDto);
            return Ok(existingProduct.ToProductDto());
        }

        [HttpDelete("{ProductId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int ProductId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productModel = await _productRepo.DeleteAsync(ProductId);
            if (productModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}