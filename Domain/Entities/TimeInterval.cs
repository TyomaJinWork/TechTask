namespace Domain.Entities
{
    public class TimeInterval
    {
        public const int DURATION_IN_MINUTES = 30;

        public int TimeIntervalId { get; set; }
        public int WindowId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Window Window { get; set; } = null!;
        public Price? Price { get; set; }
    }
}
