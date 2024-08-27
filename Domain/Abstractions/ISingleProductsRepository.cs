using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ISingleProductsRepository
    {
        Task<Result<SingleProduct>> GetAsync(int id);
    }
}