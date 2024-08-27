using CSharpFunctionalExtensions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class PricesRepository : IPricesRepository
    {
        private readonly AppDbContext _context;

        public PricesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Price>> CreateAsync(Price price)
        {
            var priceEntity = await _context.Price.AddAsync(price);

            await _context.SaveChangesAsync();

            return Result.Success(priceEntity.Entity);
        }

        public async Task<Result> CheckAvailability(SingleProduct singleProduct, DateTime windowStartDate, DateTime windowEndDate,
            TimeSpan intervalStartTime, TimeSpan intervalEndTime)
        {
            var priceEntity = await _context.Price
                .Where(x => x.TimeInterval.StartTime < intervalEndTime && intervalStartTime < x.TimeInterval.EndTime
                    && x.Window.StartDate < windowEndDate && windowStartDate < x.Window.EndDate
                    && x.Window.SingleProductId == singleProduct.SingleProductId)
                .ToListAsync();

            if (priceEntity.Count > 0)
                return Result.Failure($"You've already set up price for the product '{singleProduct.Name}' with the same date and time");

            return Result.Success();
        }

        public async Task<Result> CheckAvailability(ComboProduct comboProduct, DateTime windowStartDate, DateTime windowEndDate,
            TimeSpan intervalStartTime, TimeSpan intervalEndTime)
        {
            List<int> singleIds = comboProduct.SingleProducts.Select(x => x.SingleProductId).ToList();

            var priceEntity = await _context.Price
                .Where(x => x.TimeInterval.StartTime < intervalEndTime && intervalStartTime < x.TimeInterval.EndTime
                    && x.Window.StartDate < windowEndDate && windowStartDate < x.Window.EndDate && (x.Window.ComboProductId.HasValue
                    && x.Window.ComboProductId.Value == comboProduct.ComboProductId || singleIds.Contains(x.Window.SingleProductId)))
                .ToListAsync();

            if (priceEntity.Count > 0)
                return Result.Failure($"You've already set up price for the product '{comboProduct.Name}', " +
                    $"or one of it's child products with the same date and time");

            return Result.Success();
        }

        public async Task<Result> ChangePrice(int windowId, int timeIntervalId, decimal price)
        {
            var priceCombo = await _context.Price
                .Where(p => p.WindowId == windowId && p.TimeIntervalId == timeIntervalId && p.Window.ComboProduct != null)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.Value, p => price * p.Window.ComboProduct!.SingleProducts.Count));

            var priceSingle = await _context.Price
                .Where(p => p.WindowId == windowId && p.TimeIntervalId == timeIntervalId && p.Window.SingleProduct != null)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.Value, p => price));

            if (priceCombo == 0 && priceSingle == 0)
                return Result.Failure($"Can't find prices for the window: '{windowId}' and time interval '{timeIntervalId}'");

            return Result.Success();
        }
    }
}
