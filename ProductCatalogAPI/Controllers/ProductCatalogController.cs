using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCatalogController : ControllerBase
    {
        private readonly IProductCatalogService ProductCataloService;

        public ProductCatalogController(IProductCatalogService productCataloService)
        {
            ProductCataloService = productCataloService;
        }
        [HttpGet("{productId}")]
        public async Task<ActionResult> LookupProduct(int productId)
        {
            var isFound = await ProductCataloService.LookupProduct(productId); //todo:
            if (isFound == false)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var result = await ProductCataloService.GetAllProducts();
            if (result == null)
                return NotFound();
            var products = new  List<ProductDTO>();
            foreach (var item in result)
            {
                var productDTO = new ProductDTO
                {
                    productId = item.Id,
                    Title = item.Title,
                    Price= item.Price,
                    Inventory   = item.Quantity
                };

                products.Add(productDTO);
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddProduct(int productId, string title, decimal price)
        {
            var product = new Product(productId, title, price);
            var result = await ProductCataloService.AddProduct(product);
            return Ok(result.ProductId);
        }
    }
}
