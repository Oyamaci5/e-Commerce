using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ParanumusTask.Models
{
	public class Purchase
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int PurchaseId { get; set; }
		[ForeignKey("Users")]
		public int UserId { get; set; }
		[ForeignKey("Product")]
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime PurchaseDate { get; set; }
	}

}
