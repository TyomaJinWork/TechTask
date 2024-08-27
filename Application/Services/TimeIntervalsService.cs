using CSharpFunctionalExtensions;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class TimeIntervalsService : ITimeIntervalsService
    {
        private readonly ITimeIntervalsRepository _timeIntervalRepository;
        private readonly ILogger<TimeIntervalsService> _logger;

        public TimeIntervalsService(ITimeIntervalsRepository timeIntervalRepository, ILogger<TimeIntervalsService> logger)
        {
            _timeIntervalRepository = timeIntervalRepository;
            _logger = logger;
        }

        public async Task<Result<List<TimeInterval>>> GenerateTimeIntervalsAsync(Window window, TimeOnly startTime, TimeOnly endTime)
        {
            var timeIntervals = new List<TimeInterval>();

            var duration = TimeSpan.FromMinutes(TimeInterval.DURATION_IN_MINUTES);

            TimeSpan createStartTime = startTime.ToTimeSpan();
            TimeSpan createEndTime = startTime.ToTimeSpan() + duration;

            while (createEndTime <= endTime.ToTimeSpan())
            {
                timeIntervals.Add(new TimeInterval
                {
                    WindowId = window.WindowId,
                    Window = window,
                    StartTime = createStartTime,
                    EndTime = createEndTime,
                });

                createStartTime += duration;
                createEndTime += duration;
            }

            if (timeIntervals.Count == 0)
            {
                var message = $"Can't create intervals between {startTime} and {endTime}, duration should be at least {TimeInterval.DURATION_IN_MINUTES} minutes";

                _logger.LogWarning("Fail: {Message}", message);
                return Result.Failure<List<TimeInterval>>(message);
            }

            var result = await _timeIntervalRepository.CreateListAsync(timeIntervals);

            if (result.IsFailure)
            {
                _logger.LogWarning("Fail: {Message}", result.Error);
                return Result.Failure<List<TimeInterval>>(result.Error);
            }

            return Result.Success(result.Value);
        }
    }
}
