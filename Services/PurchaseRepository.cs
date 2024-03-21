using Microsoft.EntityFrameworkCore;
using ParanumusTask.Data;
using ParanumusTask.Models;

namespace ParanumusTask.Services
{
	public class PurchaseRepository : IPurchaseService
	{
		private readonly ParanumusTaskContext _context;

		public PurchaseRepository(ParanumusTaskContext context)
		{
			_context = context;
		}
		public async Task<Product> AddToCart(int productId)
		{
			return await _context.Products.FirstOrDefaultAsync(u => u.Id == productId);
		}

		public (decimal, decimal) GetCartPrice(int userId, decimal totalPrice, List<string> roles)
		{
			var purchasedTotalPrice = _context.Purchase
				.Where(u => u.UserId == userId)
				.Sum(u => u.TotalPrice);

			decimal discountPercentage = 0;

			if (purchasedTotalPrice > 100 && roles.Contains("Customer"))
			{
				discountPercentage = 0.1m; // 10% discount for premium customers
			}
			if (roles.Contains("Employee"))
			{
				discountPercentage = 0.3m; // 30% discount for employees
			}

			decimal discountedPrice = totalPrice * (1 - discountPercentage);

			return (discountedPrice , discountPercentage);
		}


		public async Task PurchaseProducts(List<Product> products, int userId)
		{
			var groupedProducts = products.GroupBy(p => p.Id);
			foreach (var group in groupedProducts)
			{
		
				var purchase = new Purchase
				{
					UserId = userId,
					ProductId = group.Key,
					Quantity = group.Count(),
					TotalPrice = group.Sum(u => u.Price),
					PurchaseDate = DateTime.UtcNow 
				};

				_context.Purchase.Add(purchase);

				var productToUpdate = await _context.Products.FindAsync(group.Key);
				if (productToUpdate != null)
				{
					productToUpdate.stock -= group.Count();
					_context.Products.Update(productToUpdate);
				}
			}

			// Save changes to the database
			await _context.SaveChangesAsync();
		}

		public async Task<PurchaseHistory> GetPurchaseHistory(int userId, List<string> Role)
		{
			var allPurchase = await _context.Purchase
				.Where(p => p.UserId == userId)
				.ToListAsync();

			PurchaseHistory purchaseH = new PurchaseHistory();
			List<Product> pList = new List<Product>();
			purchaseH.UserId = userId;
			decimal totalPrice = 0;
			foreach (var purchase in allPurchase)
			{
				Product p = await _context.Products.FindAsync(purchase.ProductId);
				pList.Add(p);
				totalPrice += p.Price;
			}
			purchaseH.ProductList = pList;
			var (d_price, d_percentage) = GetCartPrice(userId, totalPrice, Role);
			purchaseH.AmountPaid = d_price;
			purchaseH.TotalPrice = totalPrice;
			return purchaseH;
		}
	}
}
