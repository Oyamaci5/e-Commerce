using Microsoft.EntityFrameworkCore;
using Moq;
using ParanumusTask.Data;
using ParanumusTask.Models;
using ParanumusTask.Services;
using Xunit;

namespace ParanumusTask.Tests
{
	public class ProductRepositoryTests
	{
		[Fact]
		public async Task GetAllAsync_ReturnsAllProducts()
		{
			// Arrange
			var mockDbSet = new Mock<DbSet<Product>>();
			var products = new List<Product>
			{
				new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10.0m },
				new Product { Id = 2, Name = "Product2", Description = "Description2", Price = 20.0m },
			}.AsQueryable();

			mockDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
			mockDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
			mockDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
			mockDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

			var mockContext = new Mock<ParanumusTaskContext>();
			mockContext.Setup(m => m.Products).Returns(mockDbSet.Object);

			var productRepository = new ProductRepository(mockContext.Object);

			// Act
			var result = await productRepository.GetAllAsync();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async Task GetAsync_ExistingId_ReturnsProduct()
		{
			// Arrange
			var mockDbSet = new Mock<DbSet<Product>>();
			var existingProduct = new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10.0m };

			mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(existingProduct);

			var mockContext = new Mock<ParanumusTaskContext>();
			mockContext.Setup(m => m.Products).Returns(mockDbSet.Object);

			var productRepository = new ProductRepository(mockContext.Object);

			// Act
			var result = await productRepository.GetAsync(1);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(existingProduct, result);
		}

		[Fact]
		public async Task AddAsync_ValidProduct_ReturnsAddedProduct()
		{
			// Arrange
			var mockDbSet = new Mock<DbSet<Product>>();
			var mockContext = new Mock<ParanumusTaskContext>();
			mockContext.Setup(m => m.Products).Returns(mockDbSet.Object);

			var productRepository = new ProductRepository(mockContext.Object);

			var productToAdd = new Product { Id = 3, Name = "Product3", Description = "Description3", Price = 30.0m };

			// Act
			var addedProduct = await productRepository.AddAsync(productToAdd);

			// Assert
			Assert.NotNull(addedProduct);
			Assert.Equal(productToAdd, addedProduct);
		}

		[Fact]
		public async Task RemoveAsync_ExistingId_RemovesProduct()
		{
			// Arrange
			var existingProduct = new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10.0m };

			var mockDbSet = new Mock<DbSet<Product>>();
			mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(existingProduct);

			var mockContext = new Mock<ParanumusTaskContext>();
			mockContext.Setup(m => m.Products).Returns(mockDbSet.Object);

			var productRepository = new ProductRepository(mockContext.Object);

			// Act
			await productRepository.RemoveAsync(1);

			// Assert
			mockDbSet.Verify(m => m.Remove(It.IsAny<Product>()), Times.Once);
		}

		[Fact]
		public async Task UpdateAsync_ExistingProduct_ReturnsTrue()
		{
			// Arrange
			var existingProduct = new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10.0m };

			var mockDbSet = new Mock<DbSet<Product>>();
			mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(existingProduct);

			var mockContext = new Mock<ParanumusTaskContext>();
			mockContext.Setup(m => m.Products).Returns(mockDbSet.Object);

			var productRepository = new ProductRepository(mockContext.Object);

			var updatedProduct = new Product { Id = 1, Name = "UpdatedProduct", Description = "UpdatedDescription", Price = 20.0m };

			// Act
			var result = await productRepository.UpdateAsync(updatedProduct);

			// Assert
			Assert.True(result);
			Assert.Equal(updatedProduct.Name, existingProduct.Name);
			Assert.Equal(updatedProduct.Description, existingProduct.Description);
			Assert.Equal(updatedProduct.Price, existingProduct.Price);
		}
	}
}
