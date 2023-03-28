using Domain.Entities;
using Microsoft.AspNetCore.Http;
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
    }
}
