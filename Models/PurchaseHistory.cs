namespace ParanumusTask.Models
{
	public class PurchaseHistory
	{
		public int UserId { get; set; }
		public List<Product> ProductList { get; set; }
		public decimal AmountPaid { get; set; }
		public decimal TotalPrice { get; set; }
	}

}
