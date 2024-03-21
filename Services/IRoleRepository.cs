using System.Threading.Tasks;
using ParanumusTask.Models;

namespace ParanumusTask.Services
{
    public interface IRoleRepository
    {
        Task<Roles> GetRoleByNameAsync(string name);
        Task<int> GetRoleIdAsync(string roleName);
        Task AddRoleAsync(Roles role);
        Task DeleteRoleAsync(int roleId);
        Task UpdateRoleAsync(Roles role);
    }
}
