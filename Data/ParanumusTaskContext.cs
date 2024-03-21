using Microsoft.EntityFrameworkCore;
using ParanumusTask.Models;

namespace ParanumusTask.Data
{
    public class ParanumusTaskContext : DbContext
    {
        public ParanumusTaskContext (DbContextOptions<ParanumusTaskContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = default!;
		public DbSet<Users> Users { get; set; }
		public DbSet<Roles> Roles { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<Purchase> Purchase { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserRole>()
				.HasKey(ur => new { ur.UserId, ur.RoleId });
		}


	}
}
