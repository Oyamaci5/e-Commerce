using System.Threading.Tasks;
using ParanumusTask.Models;

namespace ParanumusTask.Services
{
    public interface IUserRepository
    {
        Task<Users> GetUserByIdAsync(int userId);
        Task<Users> GetUserByUsernameAsync(string username);
        Task AddUserAsync(Users user);
        Task<Users> RegisterUserAsync(Users user, Roles role);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(int userId);
        Task<UserWithRoleDTO> GetUserByCredentialsAsync(string username, string password);
		bool UserRoleAdding(int userId, string newRole);
        bool UserRoleRemoving(int userId, string oldRole);
		Task<List<ChangeRoleModel>> GetAllUsers();
	}
}
