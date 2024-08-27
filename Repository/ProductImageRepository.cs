using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDBContext _context;

        public ProductImageRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ProductImage> CreateAsync(IFormFile imageFile, int productId)
        {
            // 1. Retrieve the Product
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            // 2. Get the count of existing images for the product
            var existingImagesCount = await _context.ProductImages
                                                     .Where(pi => pi.ProductId == productId)
                                                     .CountAsync();

            // 3. Generate the new sequential serial number
            var serialNumber = (existingImagesCount + 1).ToString();

            // 4. Create a specific folder for the product inside wwwroot/images
            var productFolder = Path.Combine("wwwroot", "images", product.Name);
            if (!Directory.Exists(productFolder))
            {
                Directory.CreateDirectory(productFolder);  // Create the directory if it doesn't exist
            }

            // 5. Construct the new file name using the product name and sequential serial number
            var fileName = $"{product.Name}_{serialNumber}{Path.GetExtension(imageFile.FileName)}";

            // 6. Define the path to save the image inside the product-specific folder
            var imagePath = Path.Combine(productFolder, fileName);

            // 7. Save the image to the specified path
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // 8. Create a new ProductImage record
            var productImage = new ProductImage
            {
                ProductId = productId,
                ImagePath = Path.Combine("images", product.Name, fileName),  // Save the relative path to the database
            };

            // 9. Save the ProductImage record to the database
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();

            return productImage;
        }

        public async Task<ProductImage> DeleteAsync(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage == null)
            {
                throw new ArgumentException("Product image not found");
            }

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();

            // Optionally delete the file from the server
            if (File.Exists(productImage.ImagePath))
            {
                File.Delete(productImage.ImagePath);
            }

            return productImage;
        }

        public async Task<List<ProductImage>> GetAllAsync()
        {
            return await _context.ProductImages.ToListAsync();
        }

        public async Task<ProductImage> GetByIdAsync(int ProductId)
        {
            return await _context.ProductImages.FindAsync(ProductId);
        }

        public async Task<ProductImage> UpdatedAsync(int id, ProductImage productImageModel)
        {
            var existingProductImage = await _context.ProductImages.FindAsync(id);
            if (existingProductImage == null)
            {
                throw new ArgumentException("Product image not found");
            }

            existingProductImage.ImagePath = productImageModel.ImagePath;
            _context.ProductImages.Update(existingProductImage);
            await _context.SaveChangesAsync();

            return existingProductImage;
        }

        public async Task RemoveAsync(ProductImage productImage)
        {
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
        }

    }
}
