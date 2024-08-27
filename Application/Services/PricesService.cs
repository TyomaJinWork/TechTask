using CSharpFunctionalExtensions;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace Application.Services
{
    public class PricesService : IPricesService
    {
        private readonly IPricesRepository _pricesRepository;
        private readonly ILogger<PricesService> _logger;

        public PricesService(IPricesRepository pricesRepository, ILogger<PricesService> logger)
        {
            _pricesRepository = pricesRepository;
            _logger = logger;
        }

        public async Task<Result> AddPricesAsync(SingleProduct singleProduct, Window window, List<TimeInterval> timeIntervals, decimal price)
        {
            var errorMessages = new StringBuilder();

            foreach (var timeInterval in timeIntervals)
            {
                var result = await _pricesRepository.CreateAsync(new Price
                {
                    WindowId = window.WindowId,
                    Window = window,
                    TimeIntervalId = timeInterval.TimeIntervalId,
                    TimeInterval = timeInterval,
                    Value = price,
                });

                if (result.IsFailure)
                    errorMessages.AppendLine(result.Error);
            }

            if (errorMessages.Length > 0)
            {
                _logger.LogWarning("Fail: {Message}", errorMessages.ToString());
                return Result.Failure(errorMessages.ToString());
            }

            return Result.Success();
        }

        public async Task<Result> AddPricesAsync(ComboProduct comboProduct, Window window, List<TimeInterval> timeIntervals, decimal price)
        {
            var errorMessages = new StringBuilder();

            foreach (var timeInterval in timeIntervals)
            {
                var result = await _pricesRepository.CreateAsync(new Price
                {
                    WindowId = window.WindowId,
                    Window = window,
                    TimeIntervalId = timeInterval.TimeIntervalId,
                    TimeInterval = timeInterval,
                    Value = price / comboProduct.SingleProducts.Count,
                });

                if (result.IsFailure)
                    errorMessages.AppendLine(result.Error);
            }

            if (errorMessages.Length > 0)
            {
                _logger.LogWarning("Fail: {Message}", errorMessages.ToString());
                return Result.Failure(errorMessages.ToString());
            }

            return Result.Success();
        }

        public async Task<Result<bool>> ChangePriceAsync(int windowId, int timeIntervalId, decimal price)
        {
            var result = await _pricesRepository.ChangePrice(windowId, timeIntervalId, price);

            if (result.IsFailure)
            {
                _logger.LogWarning("Fail: {Message}", result.Error);
                return Result.Failure<bool>(result.Error);
            }
            
            return Result.Success(true);
        }
    }
}
