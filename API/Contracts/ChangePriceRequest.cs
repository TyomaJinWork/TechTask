namespace API.Contracts
{
    public class ChangePriceRequest
    {
        public required int WindowId { get; set; }
        public required int TimeIntervalId { get; set; }
        public decimal Price { get; set; }
    }
}
