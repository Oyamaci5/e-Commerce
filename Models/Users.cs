using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ParanumusTask.Models
{
	public class Users
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}

}
