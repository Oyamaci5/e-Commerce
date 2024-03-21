namespace ParanumusTask.Models
{
	public class ChangeRoleModel
	{
		public string Username { get; set; }

		public int UserId { get; set; }	
		public List<string> RoleName { get; set; }

	}
}
