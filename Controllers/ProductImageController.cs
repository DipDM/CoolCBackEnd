using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolCBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductImageController(IProductImageRepository productImageRepository, IWebHostEnvironment webhostEnvironment)
        {
            _productImageRepository = productImageRepository;
            _webHostEnvironment = webhostEnvironment;
        }

        [HttpPost("{productId:int}")]
        public async Task<IActionResult> UploadImage(int productId, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("Image file is missing");
            }

            var productImage = await _productImageRepository.CreateAsync(imageFile, productId);

            return Ok(productImage);
        }

        [HttpPut("{productImageId:int}")]
        public async Task<IActionResult> UpdateImage(int productImageId, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("Image File is missing");
            }

            var productImage = await _productImageRepository.UpdateAsync(productImageId,imageFile);

            return Ok(productImage);
        }



        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _productImageRepository.GetAllAsync();
            return Ok(images);
        }

        [HttpGet("{productImageId:int}")]
        public async Task<IActionResult> GetImageById(int productImageId)
        {
            var image = await _productImageRepository.GetByIdAsync(productImageId);
            if (image == null)
            {
                return NotFound();
            }
            return Ok(image);
        }

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var images = await _productImageRepository.GetByProductIdAsync(productId);
            if(images == null)
            {
                return NotFound();
            }
            return Ok(images);
        }

        [HttpDelete("{productImageId:int}")]
        public async Task<IActionResult> DeleteImage(int productImageId)
        {
            try
            {
                var productImage = await _productImageRepository.GetByIdAsync(productImageId);

                if (productImage == null)
                {
                    return NotFound();
                }

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, productImage.ImagePath);

                Console.WriteLine($"Deleting file at path: {filePath}");

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                else
                {
                    Console.WriteLine("File Does not exists");
                }

                await _productImageRepository.RemoveAsync(productImage);
                return Ok(productImage);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server error:{e.Message}");
            }
        }




    }
}
