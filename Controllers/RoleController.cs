using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParanumusTask.Models;
using ParanumusTask.Services;

namespace ParanumusTask.Controllers
{
    [Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : Controller
	{
		private readonly IConfiguration _config;
		private readonly IRoleRepository _roleRepository;

		public RoleController(IConfiguration config, IRoleRepository roleRepository)
		{
			_config = config;
			_roleRepository = roleRepository;
		}

		[HttpPost("add")]
		public async Task<IActionResult> AddRoleAsync(string roleName)
		{
			var existingRole = await _roleRepository.GetRoleByNameAsync(roleName);
			if (existingRole != null)
			{
				return Conflict("Role already exists");
			}

			var newRole = new Roles
			{
				Name = roleName
			};

			await _roleRepository.AddRoleAsync(newRole);

			return Ok("Role added successfully");
		}

		[HttpDelete("delete/{name}")]
		public async Task<IActionResult> DeleteRoleAsync(string name)
		{
			var existingRole = await _roleRepository.GetRoleByNameAsync(name);

			if (existingRole == null)
			{
				return NotFound(); // 404 Not Found
			}

			try
			{
				await _roleRepository.DeleteRoleAsync(existingRole.RoleId); // Delete the role from the repository
				return Ok("Role Delete Successfully."); // 204 No Content for successful deletion
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal Server Error: {ex.Message}"); // 500 Internal Server Error
			}
		}

		[HttpPut("role/{name}")]
		public async Task<IActionResult> UpdateRoleAsync(string name, string newName)
		{
			var existingRole = await _roleRepository.GetRoleByNameAsync(name);

			if (existingRole == null)
			{
				return NotFound(); // 404 Not Found
			}

			existingRole.Name = newName;

			try
			{
				await _roleRepository.UpdateRoleAsync(existingRole);
				return Ok("Role Update Successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal Server Error: {ex.Message}"); // 500 Internal Server Error
			}
		}
	}
}
