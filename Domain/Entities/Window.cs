namespace Domain.Entities
{
    public class Window
    {
        public int WindowId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SingleProductId { get; set; }
        public int? ComboProductId { get; set; } = null;

        public SingleProduct SingleProduct { get; set; } = null!;
        public ComboProduct? ComboProduct { get; set; } = null;
        public List<TimeInterval> TimeIntervals { get; set; } = new List<TimeInterval>();
        public List<Price> Prices { get; set; } = null!;
    }
}
