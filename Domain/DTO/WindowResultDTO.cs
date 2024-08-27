namespace Domain.DTO
{
    public class WindowResultDTO
    {
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public List<TimeIntervalPriceResultDTO> TimeIntervalPrices { get; set; } = new List<TimeIntervalPriceResultDTO>();
    }
}
