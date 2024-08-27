using CSharpFunctionalExtensions;
using Domain.DTO;
using Domain.Entities;

namespace Application.Services
{
    public interface IPricesService
    {
        Task<Result> AddPricesAsync(ComboProduct comboProduct, Window window, List<TimeInterval> timeIntervals, decimal price);
        Task<Result> AddPricesAsync(SingleProduct singleProduct, Window window, List<TimeInterval> timeIntervals, decimal price);
        Task<Result<bool>> ChangePriceAsync(int windowId, int timeIntervalId, decimal price);
    }
}