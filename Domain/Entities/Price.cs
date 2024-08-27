namespace Domain.Entities
{
    public class Price
    {
        public int PriceId { get; set; }
        public int WindowId { get; set; }
        public int TimeIntervalId { get; set; }
        public required decimal Value { get; set; }

        public Window Window { get; set; } = null!;
        public TimeInterval TimeInterval { get; set; } = null!;
    }
}
