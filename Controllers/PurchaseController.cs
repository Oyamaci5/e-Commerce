using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParanumusTask.Models;
using ParanumusTask.Services;

namespace ParanumusTask.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class PurchaseController : ControllerBase
	{
		private readonly IPurchaseService _purchaseService;
		private readonly ICacheService _cacheService;
		private List<Product> _productList;

		public PurchaseController(IPurchaseService purchaseService, ICacheService cacheService)
		{
			_purchaseService = purchaseService;
			_cacheService = cacheService;
			_productList = new List<Product>();
		}

		[HttpPost("addtocart")]
		public async Task<IActionResult> AddToCart(int productId)
		{
			Product product = await _purchaseService.AddToCart(productId);
			if (product != null)
			{
				int userId = _cacheService.GetData<int>("userId");
				_productList = _cacheService.GetData<List<Product>>(userId.ToString());

				var expiryDate = DateTimeOffset.Now.AddMinutes(20);
				_productList.Add(product);
				_cacheService.SetData(userId.ToString(), _productList, expiryDate);

				_productList = _cacheService.GetData<List<Product>>(userId.ToString());
				return Ok(new
				{
					message = $"{product.Name} added to cart successfully",
					productList = _productList
				});
			}
			else
			{
				return NotFound();
			}
		}
		[HttpGet("showcart")]
		public IActionResult ShowCart() 
		{
			int userId = _cacheService.GetData<int>("userId");

			// Get the cart items from the cache based on userId
			List<Product> cartItems = _cacheService.GetData<List<Product>>(userId.ToString());

			if (cartItems != null)
			{
				return Ok(cartItems);
			}
			else
			{
				return NotFound("Cart is empty.");
			}
		}
		[HttpPost("removefromcart")]
		public IActionResult RemoveFromCart(int productId)
		{
			int userId = _cacheService.GetData<int>("userId");
			List<Product> productList = _cacheService.GetData<List<Product>>(userId.ToString());

			if (productList != null)
			{
				Product productToRemove = productList.FirstOrDefault(p => p.Id == productId);
				if (productToRemove != null)
				{
					productList.Remove(productToRemove);
					_cacheService.SetData(userId.ToString(), productList, DateTimeOffset.Now.AddMinutes(20));
					_productList = _cacheService.GetData<List<Product>>(userId.ToString());
					return Ok(new {
						message = $"{productToRemove.Name} removed from cart successfully",
						productList = _productList
					});

				}
				else
				{
					return NotFound("Product not found in cart");
				}
			}
			else
			{
				return NotFound("Cart not found for the user");
			}
		}

		[HttpGet("getcartprice")]
		public async Task<IActionResult> GetCartPrice()
		{
			int userId = _cacheService.GetData<int>("userId");
			_productList = _cacheService.GetData<List<Product>>(userId.ToString());
			var totalPrice = _productList.Sum(product => product.Price);
			var role = _cacheService.GetData<List<string>>("role");
			var (d_price, d_percentage) = _purchaseService.GetCartPrice(userId, totalPrice, role);
			var responseString = $"Total Price: {totalPrice}\nDiscounted Price: {d_price}\nDiscount Percentage: {d_percentage:P}";
			return Ok(responseString);
		}

		[HttpPost("purchaseproducts")]
		public async Task<IActionResult> PurchaseProducts()
		{
			int userId = _cacheService.GetData<int>("userId");
			_productList = _cacheService.GetData<List<Product>>(userId.ToString());
			await _purchaseService.PurchaseProducts(_productList, userId);
			return Ok("Products purchased successfully");
		}

		[HttpPost("purchasehistory")]
		public async Task<IActionResult> GetPurchaseHistory()
		{
			var UserId = _cacheService.GetData<int>("userId");
			var role = _cacheService.GetData<List<string>>("role");
			var result = await _purchaseService.GetPurchaseHistory(UserId, role);
			
			// Validate the purchase history data
			if (result == null)
			{
				return BadRequest("Invalid purchase history data.");
			}
			var formattedResult = $"User ID: {result.UserId}\n";
			formattedResult += "Product List:\n";
			foreach (var product in result.ProductList)
			{
				formattedResult += $"- {product.Name}, Price: {product.Price}\n";
			}
			formattedResult += $"Amount Paid: {result.TotalPrice}\n";
			formattedResult += $"Amount Paid: {result.AmountPaid}";

			return Ok($"Purchase history \n {formattedResult}.");
		}
	}
}
