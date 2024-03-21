using Microsoft.EntityFrameworkCore;
using ParanumusTask.Data;
using ParanumusTask.Models;

namespace ParanumusTask.Services
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ParanumusTaskContext _context;

        public RoleRepository(ParanumusTaskContext context)
        {
            _context = context;
        }

        public async Task<int> GetRoleIdAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            return role?.RoleId ?? 0;
        }

        public async Task<Roles> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task AddRoleAsync(Roles role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateRoleAsync(Roles role)
        {
            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == role.RoleId);
            if (existingRole != null)
            {
                // Update role properties here if needed
                await _context.SaveChangesAsync();
            }
        }
    }
}
