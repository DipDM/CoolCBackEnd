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

            // 2. Retrieve the Brand using the brandId from the Product
            var brand = await _context.Brands.FindAsync(product.BrandId);
            if (brand == null)
            {
                throw new ArgumentException("Brand not found");
            }

            // 3. Get the count of existing images for the product
            var existingImagesCount = await _context.ProductImages
                                                    .Where(pi => pi.ProductId == productId)
                                                    .CountAsync();

            // 4. Generate the new sequential serial number
            var serialNumber = (existingImagesCount + 1).ToString();

            // 5. Create a specific folder for the product inside wwwroot/images
            var folderName = $"{brand.Name}_{product.Name}";
            var productFolder = Path.Combine(@"D:\gram\development\Complete\CoolC\CoolCBackend\wwwroot\images", folderName);
            if (!Directory.Exists(productFolder))
            {
                Directory.CreateDirectory(productFolder);  // Create the directory if it doesn't exist
            }

            // 6. Construct the new file name using the brand name, product name, and sequential serial number
            var fileName = $"{folderName}_{serialNumber}{Path.GetExtension(imageFile.FileName)}";

            // 7. Define the path to save the image inside the product-specific folder
            var imagePath = Path.Combine(productFolder, fileName);

            // 8. Save the image to the specified path
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // 9. Create a new ProductImage record
            var productImage = new ProductImage
            {
                ProductId = productId,
                ImagePath = Path.Combine("images", folderName, fileName),  // Save the relative path to the database
            };

            // 10. Save the ProductImage record to the database
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();

            return productImage;
        }
        public async Task<ProductImage> DeleteAsync(int productImageId)
        {
            var productImage = await _context.ProductImages.FindAsync(productImageId);
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

        public async Task RemoveAsync(ProductImage productImage)
        {
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductImage> UpdateAsync(int productImageId, IFormFile newImageFile)
        {
            var existingProductImage = await _context.ProductImages.FindAsync(productImageId);
            if (existingProductImage == null)
            {
                throw new ArgumentException("Product image not found");
            }

            var product = await _context.Products.FindAsync(existingProductImage.ProductId);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            var brand = await _context.Brands.FindAsync(product.BrandId);
            if (brand == null)
            {
                throw new ArgumentException("Brand not found");
            }

            var existingImagePath = Path.Combine(@"D:\gram\development\Complete\CoolC\CoolCBackend\wwwroot", existingProductImage.ImagePath);
            if (File.Exists(existingImagePath))
            {
                File.Delete(existingImagePath);
            }

            var folderName = $"{brand.Name}_{product.Name}";
            var productFolder = Path.Combine(@"D:\gram\development\Complete\CoolC\CoolCBackend\wwwroot\images", folderName);
            if (!Directory.Exists(productFolder))
            {
                Directory.CreateDirectory(productFolder);
            }

            var fileName = $"{folderName}{Path.GetExtension(newImageFile.FileName)}";
            var newImagePath = Path.Combine(productFolder, fileName);

            using (var stream = new FileStream(newImagePath, FileMode.Create))
            {
                await newImageFile.CopyToAsync(stream);
            }

            existingProductImage.ImagePath = Path.Combine("images", folderName, fileName);

            _context.ProductImages.Update(existingProductImage);
            await _context.SaveChangesAsync();

            return existingProductImage;
        }

    }
}
