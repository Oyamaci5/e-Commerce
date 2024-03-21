using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ParanumusTask.Models
{
	public class Product			
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }

		public int stock { get; set; }
	}
}
