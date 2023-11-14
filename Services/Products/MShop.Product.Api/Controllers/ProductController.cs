using Microsoft.AspNetCore.Mvc;
using MShop.Infrastructure.Command.Product;
using MShop.Product.DataProvider.Services;

namespace MShop.Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string productId)
        {
            var product = await _productService.GetProduct(productId);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProduct product)
        {
            var adddedProduct = await _productService.AddProduct(product);
            return Ok(adddedProduct);
        }
    }
}
