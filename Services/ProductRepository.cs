using Microsoft.EntityFrameworkCore;
using ParanumusTask.Data;
using ParanumusTask.Models;
using System;

namespace ParanumusTask.Services
{
    public class ProductRepository : IProductRepository
    {

        private readonly ParanumusTaskContext _context;

        public ProductRepository(ParanumusTaskContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Products.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task RemoveAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateAsync(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var existingProduct = await _context.Products.FindAsync(item.Id);
            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.Name = item.Name;
            existingProduct.Description = item.Description;
            existingProduct.Price = item.Price;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
