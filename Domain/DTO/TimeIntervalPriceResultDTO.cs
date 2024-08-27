namespace Domain.DTO
{
    public class TimeIntervalPriceResultDTO
    {
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
