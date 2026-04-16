using Microsoft.AspNetCore.Mvc;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.ServiceInterfaces;

namespace WSC.Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service) => _service = service;

        [HttpGet("GetAll")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductResponseDto>>>> GetAllProducts(CancellationToken ct)
        {
            var products = await _service.GetAllProductsAsync(ct);
            return Ok(products);

        }
        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse<int>>> CreateProduct([FromBody] CreateProductDto dto, CancellationToken ct)
        {
            var result = await _service.CreateProductAsync(dto, ct);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id, CancellationToken ct)
        {
            var result = await _service.DeleteProductAsync(id, ct);
            if (result.Success)
                return Ok(result);
            return BadRequest("Unable to delete product.");
        }
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetProductById(int id, CancellationToken ct)
        {
            var result = await _service.GetProductByIdAsync(id, ct);
            if (result.Success)
                return Ok(result);
            return NotFound($"Product with ID {id} not found.");
        }
        [HttpPut("update")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProduct([FromBody] UpdateProductDto dto, CancellationToken ct)
        {
            var result = await _service.UpdateProductAsync(dto, ct);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }
    }
}
