using CSharpFunctionalExtensions;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SingleProductsRepository : ISingleProductsRepository
    {
        private readonly AppDbContext _context;

        public SingleProductsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<SingleProduct>> GetAsync(int id)
        {
            var result = await _context.SingleProduct
                .FirstOrDefaultAsync(p => p.SingleProductId == id);

            if (result == null)
                return Result.Failure<SingleProduct>($"{typeof(SingleProduct)} with such Id is not found: '{id}'");

            return Result.Success(result);
        }
    }
}
