using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParanumusTask.Models;
using ParanumusTask.Services;

namespace ParanumusTask.Controllers
{

    [Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private IProductRepository _repository;

		public ProductsController(IProductRepository repository)
		{
			_repository = repository;
		}
		[HttpGet]
		public async Task<IEnumerable<Product>> GetAllProducts()
		{
			return await _repository.GetAllAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _repository.GetAsync(id);
			if (product == null)
			{
				return BadRequest(); // Return 404 Not Found if product is not found
			}

			return Ok(product);
		}
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult<Product>> AddProduct(Product product)
		{
			var addedProduct = await _repository.AddAsync(product);
			return CreatedAtAction(nameof(GetProduct), new { id = addedProduct.Id }, addedProduct);
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, Product product)
		{

			product.Id = id;
			var updated = await _repository.UpdateAsync(product);
			if (!updated)
			{
				return BadRequest(); // Return 404
			}

			return Ok($"Başarılı bir şekilde güncellendi.\n {product}");
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await _repository.GetAsync(id);
			if (product == null)
			{
				return BadRequest(); // Return 404 Not Found if product is not found
			}

			await _repository.RemoveAsync(id);
			return Ok($"Başarılı bir şekilde silindi.\n {product}");
		}
	}
}
