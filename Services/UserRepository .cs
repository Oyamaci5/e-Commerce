using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParanumusTask.Data;
using ParanumusTask.Models;

namespace ParanumusTask.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ParanumusTaskContext _context;
        private readonly ICacheService _cacheService;

        public UserRepository(ParanumusTaskContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<UserWithRoleDTO> GetUserByCredentialsAsync(string username, string password)
        {
            UserWithRoleDTO userDto = new UserWithRoleDTO();

            Users user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                // Return null if user is not found
                return null;
            }

			List<Roles> roles = await GetUserRoles(user.UserId);
			userDto.Username = user.Username;
			userDto.RoleName = new List<string>();
			foreach (var role in roles)
			{
				userDto.RoleName.Add(role.Name);
			}

			var expiryDate = DateTimeOffset.Now.AddMinutes(20);
            _cacheService.SetData("userId", user.UserId, expiryDate);
			_cacheService.SetData("username", user.Username, expiryDate);
			_cacheService.SetData("role",userDto.RoleName, expiryDate);
            _cacheService.SetData(user.UserId.ToString(), new List<Product>(), expiryDate); 

            return userDto;
        }

		public async Task<List<Roles>> GetUserRoles(int userId)
		{
			var userRoles = await _context.UserRoles
				.Where(u => u.UserId == userId)
				.ToListAsync();

			if (userRoles != null && userRoles.Count > 0)
			{
				var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
				return await _context.Roles
					.Where(r => roleIds.Contains(r.RoleId))
					.ToListAsync();
			}

			return new List<Roles>();
		}


		public async Task AddUserAsync(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Users> RegisterUserAsync(Users user, Roles role)
        {
            RoleRepository roleRepo = new RoleRepository(_context);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var existingRole = await roleRepo.GetRoleByNameAsync(role.Name);
            if (existingRole == null)
            {
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
            }

            int userId = (await GetUserByUsernameAsync(user.Username)).UserId;
            int roleId = await roleRepo.GetRoleIdAsync(role.Name);

            UserRole userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

		public bool UserRoleAdding(int userId, string newRole)
		{
            
			var role = _context.Roles.FirstOrDefault(r => r.Name == newRole);
            if (role == null)
            {
				Roles registerRole = new Roles { Name = newRole };
				_context.Roles.Add(registerRole);
				_context.SaveChanges();

			}
            var checkUser = _context.Users.FirstOrDefault(r => r.UserId == userId);
            if(checkUser != null)
            {
				var roleWithUser = _context.Roles.FirstOrDefault(r => r.Name == newRole);
				UserRole ur = new UserRole { UserId = userId, RoleId = roleWithUser.RoleId };
				_context.UserRoles.Add(ur);
				_context.SaveChanges();
                return true;
			}
			else { return false; }
            
		}
		public bool UserRoleRemoving(int userId, string oldRole)
		{
			var user = _context.Users.Find(userId);
			if (user == null)
			{
				return false; // User not found
			}

			var role = _context.Roles.FirstOrDefault(r => r.Name == oldRole);
			if (role == null)
			{
				return false; // Role not found
			}

			var existingUserRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == role.RoleId);
			if (existingUserRole == null)
			{
				return false; // User does not have the specified role
			}

			_context.UserRoles.Remove(existingUserRole);
			_context.SaveChanges();

			return true; // Role removed successfully
		}
		public async Task<List<ChangeRoleModel>> GetAllUsers()
		{
			var usersRoles = await _context.UserRoles.ToListAsync();
			var usersGrouped = usersRoles.GroupBy(ur => ur.UserId);
			List<ChangeRoleModel> usersWithRoles = new List<ChangeRoleModel>();

			foreach (var userGroup in usersGrouped)
			{
				var user = await _context.Users.FindAsync(userGroup.Key);
				if (user != null)
				{
					var roles = userGroup.Select(ur => ur.RoleId)
										 .Join(_context.Roles, ur => ur, r => r.RoleId, (ur, r) => r.Name)
										 .ToList();

					ChangeRoleModel userDto = new ChangeRoleModel
					{
						Username = user.Username,
						UserId = user.UserId,
						RoleName = roles
					};

					usersWithRoles.Add(userDto);
				}
			}

			return usersWithRoles;
		}
	}
}
