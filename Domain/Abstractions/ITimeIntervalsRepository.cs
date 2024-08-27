using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ITimeIntervalsRepository
    {
        Task<Result<List<TimeInterval>>> GetByWindowIdAsync(int windowId);
        Task<Result<List<TimeInterval>>> CreateListAsync(List<TimeInterval> timeIntervals);
    }
}