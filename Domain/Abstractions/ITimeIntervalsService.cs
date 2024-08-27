using CSharpFunctionalExtensions;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ITimeIntervalsService
    {
        Task<Result<List<TimeInterval>>> GenerateTimeIntervalsAsync(Window window, TimeOnly startTime, TimeOnly endTime);
    }
}