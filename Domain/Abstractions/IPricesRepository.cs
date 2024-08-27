using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IPricesRepository
    {
        Task<Result<Price>> CreateAsync(Price price);
        Task<Result> CheckAvailability(SingleProduct singleProduct, DateTime windowStartDate,
            DateTime windowEndDate, TimeSpan intervalStartTime, TimeSpan intervalEndTime);
        Task<Result> CheckAvailability(ComboProduct comboProduct, DateTime windowStartDate,
            DateTime windowEndDate, TimeSpan intervalStartTime, TimeSpan intervalEndTime);
        Task<Result> ChangePrice(int windowId, int timeIntervalId, decimal price);
    }
}