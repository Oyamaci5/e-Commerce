using ParanumusTask.Models;

namespace ParanumusTask.Services
{
	public interface IPurchaseService
	{
		Task<Product> AddToCart(int productId);
		(decimal, decimal) GetCartPrice(int userId, decimal totalPrice, List<string> role);
		Task PurchaseProducts(List<Product> products, int userId);
		Task<PurchaseHistory> GetPurchaseHistory(int userId, List<string> Role);
	}
}
