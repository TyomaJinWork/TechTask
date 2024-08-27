using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class WindowsRepository : IWindowsRepository
    {
        private readonly AppDbContext _context;

        public WindowsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Window>> CreateAsync(Window window)
        {
            var windowEntity = await _context.Window.AddAsync(window);

            await _context.SaveChangesAsync();

            return Result.Success(windowEntity.Entity);
        }

        public async Task<Result<List<Window>>> GetInRangeAsync(DateTime startDate, DateTime endDate)
        {
            var windows = await _context.Window
                .Where(x => x.StartDate >= startDate && x.EndDate <= endDate)
                .ToListAsync();

            return Result.Success(windows);
        }

        public async Task<Result<List<Window>>> GetByProductNameAndDateRange(string productName, DateTime from, DateTime to)
        {
            var windows = await _context.Window
                .Include(x => x.ComboProduct).ThenInclude(c => c != null ? c.SingleProducts : null)
                .Include(x => x.SingleProduct)
                .Include(x => x.TimeIntervals)
                .Include(x => x.Prices)
                .Where(x => x.StartDate < to && from < x.EndDate && (x.ComboProduct != null
                    && x.ComboProduct.Name == productName || x.SingleProduct != null && x.SingleProduct.Name == productName))
                .ToListAsync();

            if (windows.Count == 0)
                return Result.Failure<List<Window>>($"Can't find windows in rage: '{from}' - '{to}' for the product '{productName}'");

            return Result.Success(windows);
        }
    }
}
