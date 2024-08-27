namespace Domain.Entities
{
    public class SingleProduct
    {
        public int SingleProductId { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<ComboProduct> ComboProducts { get; set; } = new List<ComboProduct>();
        public List<Window> Windows { get; set; } = new List<Window>();
    }
}
