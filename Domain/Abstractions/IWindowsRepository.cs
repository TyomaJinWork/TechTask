using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IWindowsRepository
    {
        Task<Result<Window>> CreateAsync(Window window);
        Task<Result<List<Window>>> GetInRangeAsync(DateTime startDate, DateTime endDate);
        Task<Result<List<Window>>> GetByProductNameAndDateRange(string productName, DateTime from, DateTime to);
    }
}