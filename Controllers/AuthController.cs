using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ParanumusTask.Models;
using Microsoft.AspNetCore.Authorization;
using ParanumusTask.Services;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

	public AuthController(IConfiguration config, IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

	[HttpPost("login")]
	public async Task<IActionResult> Login(string Username, string Password)
	{
		var user = await _userRepository.GetUserByCredentialsAsync(Username, Password);

		if (user == null)
		{
			return Unauthorized("Kullanıcı şifresi veya username hatalı"); // Return 401 Unauthorized if user is not found or credentials are invalid
		}
		else if (string.IsNullOrEmpty(user.Username) || user.RoleName == null || user.RoleName.Count == 0)
		{
			return NotFound("User not registered");
		}
		else
		{
			var token = GenerateJwtToken(user);
			return Ok(new { token });
		}
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(string Username, string Password)
	{
		var existingUser = await _userRepository.GetUserByUsernameAsync(Username);
		if (existingUser != null)
		{
			return Conflict("Username already exists");
		}

		var newUser = new Users
		{
			Username = Username,
			Password = Password,
		};
		var newRole = new Roles
		{
			Name = "Customer",
		};

		await _userRepository.RegisterUserAsync(newUser, newRole);
		var addedUser = await _userRepository.GetUserByCredentialsAsync(Username, Password);

		if (addedUser == null)
		{
			return BadRequest("Failed to register user");
		}
		else
		{
			var token = GenerateJwtToken(addedUser);
			return Ok(new { token });
		}
	}

	[Authorize(Roles = "Admin")]
	[HttpPost("add")]
	public async Task<IActionResult> AddUser(string Username, string Password, string RoleName)
	{
		var existingUser = await _userRepository.GetUserByUsernameAsync(Username);
		if (existingUser != null)
		{
			return Conflict("Username already exists");
		}

		var newUser = new Users
		{
			Username = Username,
			Password = Password,
		};
		var newRole = new Roles
		{
			Name = RoleName,
		};

		await _userRepository.RegisterUserAsync(newUser, newRole);
		return Ok("New User Added.");
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("UserList")]
	public async Task<IActionResult> GetAllUsers()
	{
		var userList = await _userRepository.GetAllUsers();
		if (userList != null && userList.Count > 0)
		{
			return Ok(userList);
		}
		else
		{
			return NotFound("No users found");
		}
	}
	[Authorize(Roles = "Admin")]
	[HttpPut("addrole/{userId}")]
	public IActionResult UserRoleAdding(int userId, string newRole)
	{
		var result = _userRepository.UserRoleAdding(userId, newRole);

		if (result)
		{
			return Ok($"User role updated successfully for userId: {userId}");
		}
		else
		{
			return NotFound($"User with userId: {userId} not found");
		}
	}

	[HttpPost("removerole")]
	public IActionResult UserRoleRemoving(int userId, string existingRole)
	{
		bool result = _userRepository.UserRoleRemoving(userId, existingRole);
		if (result)
		{
			return Ok($"Role '{existingRole}' removed successfully from user with ID {userId}");
		}
		else
		{
			return NotFound("User or role not found, or user doesn't have the role.");
		}
	}
	[Authorize(Roles = "Admin")]
	[HttpPut("update/{id}")]
	public async Task<IActionResult> UpdateUser(int id, Users updatedUser)
	{
		var existingUser = await _userRepository.GetUserByIdAsync(id);

		if (existingUser == null)
		{
			return NotFound(); // 404 Not Found
		}

		existingUser.Username = updatedUser.Username;
		existingUser.Password = updatedUser.Password;

		try
		{
			await _userRepository.UpdateUserAsync(existingUser);
			return Ok("User Update Successfully");
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal Server Error: {ex.Message}"); // 500 Internal Server Error
		}
	}

	[Authorize(Roles = "Admin")]
	[HttpDelete("delete/{id}")]
	public async Task<IActionResult> DeleteUser(int id)
	{
		var existingUser = await _userRepository.GetUserByIdAsync(id);

		if (existingUser == null)
		{
			return NotFound(); // 404 Not Found
		}

		try
		{
			await _userRepository.DeleteUserAsync(existingUser.UserId); // Delete the user from the repository
			return Ok("User Delete Successfully."); // 204 No Content for successful deletion
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal Server Error: {ex.Message}"); // 500 Internal Server Error
		}
	}


	private string GenerateJwtToken(UserWithRoleDTO user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtConfig:SigningKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var claims = new List<Claim>
		{
			new Claim("Username", user.Username)
		};

		foreach (var role in user.RoleName)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		var claimsArray = claims.ToArray();

		var token = new JwtSecurityToken(
            issuer: _config["JwtConfig:Issuer"],
            audience: _config["JwtConfig:Audience"],
            claims: claimsArray,
            expires: DateTime.UtcNow.AddHours(1), // Token expiration time
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
