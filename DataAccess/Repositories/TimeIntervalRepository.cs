using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class TimeIntervalRepository : ITimeIntervalsRepository
    {
        private readonly AppDbContext _context;

        public TimeIntervalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TimeInterval>>> GetByWindowIdAsync(int windowId)
        {
            var result = await _context.TimeInterval
                .Where(i => i.WindowId >= windowId)
                .ToListAsync();

            if (result.Count == 0)
                return Result.Failure<List<TimeInterval>>($"No such {typeof(TimeInterval)}s for the provided window id: {windowId}");

            return Result.Success(result);
        }
        
        public async Task<Result<List<TimeInterval>>> CreateListAsync(List<TimeInterval> timeIntervals)
        {
            var result = new List<TimeInterval>();

            foreach (var timeInterval in timeIntervals)
            {
                var timeIntervalEntity = await _context.TimeInterval.AddAsync(timeInterval);
                result.Add(timeIntervalEntity.Entity);
            }

            await _context.SaveChangesAsync();

            return Result.Success(result);
        }
    }
}
