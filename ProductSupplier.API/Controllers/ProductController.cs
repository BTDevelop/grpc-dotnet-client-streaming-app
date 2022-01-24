using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductSupplier.API.Services;

namespace ProductSupplier.API.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly IProductSupplierService _productSupplierService;

        public ProductController(IProductSupplierService productSupplierService)
        {
            _productSupplierService = productSupplierService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadProducts(IFormFile formFile)
        {
            int insertedProductCount = await _productSupplierService.InsertRangeProductsAsync(formFile);

            return Ok($"{insertedProductCount} products have been inserted.");
        }
    }
}
