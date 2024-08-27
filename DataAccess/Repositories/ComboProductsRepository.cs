using CSharpFunctionalExtensions;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ComboProductsRepository : IComboProductsRepository
    {
        private readonly AppDbContext _context;

        public ComboProductsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ComboProduct>> GetAsync(int id)
        {
            var result = await _context.ComboProduct
                .Include(c => c.SingleProducts)
                .FirstOrDefaultAsync(p => p.ComboProductId == id);

            if (result == null)
                return Result.Failure<ComboProduct>($"{typeof(ComboProduct)} with such id is not found: '{id}'");

            if (result.SingleProducts.Count == 0)
            {
                return Result.Failure<ComboProduct>($"invalid {typeof(ComboProduct)} with id: '{id}', has no children");
            }

            return Result.Success(result);
        }
    }
}
