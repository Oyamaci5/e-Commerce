using ParanumusTask.Data;
using ParanumusTask.Models;
public static class SeedData
{
	public static void Initialize(ParanumusTaskContext context)
	{
		if (!context.Roles.Any())
		{
			context.Roles.AddRange(
				new Roles { Name = "Admin" },
				new Roles { Name = "Employee" },
				new Roles { Name = "Customer" }
			);
			context.SaveChanges();
		}

		if (!context.Users.Any())
		{
			context.Users.AddRange(
				new Users { Username = "oguzhan", Password = "admin123" },
				new Users { Username = "john", Password = "john123" },	
				new Users { Username = "premium", Password = "prem123" },
				new Users { Username = "customer", Password = "cust123" }
			);
			context.SaveChanges();
		}
		if (!context.UserRoles.Any())
		{
			context.UserRoles.AddRange(
				new UserRole { UserId = 1, RoleId = 1 },
				new UserRole { UserId = 2, RoleId = 2 },
				new UserRole { UserId = 3, RoleId = 4 },
				new UserRole { UserId = 4, RoleId = 4 },
				new UserRole { UserId = 2, RoleId = 4 } 
			);
			context.SaveChanges();
		}
		if (!context.Products.Any())
		{
			context.Products.AddRange(
				new Product { Name = "The Great Gatsby", Description = "A novel by F. Scott Fitzgerald", Price = 29.99m, stock = 100 },
				new Product { Name = "To Kill a Mockingbird", Description = "A novel by Harper Lee", Price = 19.99m, stock = 150 },
				new Product { Name = "1984", Description = "A dystopian novel by George Orwell", Price = 39.99m, stock = 80 },
				new Product { Name = "Pride and Prejudice", Description = "A novel by Jane Austen", Price = 49.99m, stock = 120 },
				new Product { Name = "The Catcher in the Rye", Description = "A novel by J.D. Salinger", Price = 34.99m, stock = 90 },
				new Product { Name = "Brave New World", Description = "A novel by Aldous Huxley", Price = 27.99m, stock = 110 },
				new Product { Name = "The Lord of the Rings", Description = "A fantasy novel by J.R.R. Tolkien", Price = 23.99m, stock = 130 },
				new Product { Name = "The Hobbit", Description = "A fantasy novel by J.R.R. Tolkien", Price = 31.99m, stock = 70 },
				new Product { Name = "Harry Potter and the Sorcerer's Stone", Description = "A fantasy novel by J.K. Rowling", Price = 41.99m, stock = 140 },
				new Product { Name = "The Da Vinci Code", Description = "A mystery thriller novel by Dan Brown", Price = 36.99m, stock = 100 }
			);
			context.SaveChanges();
		}
		if (!context.Purchase.Any())
		{
			context.Purchase.AddRange(
				new Purchase { UserId = 3, ProductId=1, Quantity=1, TotalPrice = 29.99m, PurchaseDate  = DateTime.Now.AddDays(-7) },
				new Purchase { UserId = 3, ProductId = 2, Quantity = 1, TotalPrice = 19.99m, PurchaseDate = DateTime.Now.AddDays(-7) },
				new Purchase { UserId = 3, ProductId = 3, Quantity = 1, TotalPrice = 39.99m, PurchaseDate = DateTime.Now.AddDays(-7) },
				new Purchase { UserId = 3, ProductId = 1, Quantity = 1, TotalPrice = 29.99m, PurchaseDate = DateTime.Now.AddDays(-7) }
			);
			context.SaveChanges();
		}
	}
}
