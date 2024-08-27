using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IComboProductsRepository
    {
        Task<Result<ComboProduct>> GetAsync(int id);
    }
}