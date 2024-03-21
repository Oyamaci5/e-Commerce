using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ParanumusTask.Models
{
	public class UserRole
	{
		[ForeignKey("Users")]
		public int UserId { get; set; }
		[ForeignKey("Roles")]
		public int RoleId { get; set; }
	}
}
