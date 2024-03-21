using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ParanumusTask.Models
{
	public class Roles
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RoleId { get; set; }
		public string Name { get; set; }
	}
}
