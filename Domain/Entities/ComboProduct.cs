namespace Domain.Entities
{
    public class ComboProduct
    {
        public int ComboProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SingleProduct> SingleProducts { get; set; } = new List<SingleProduct>();
        public List<Window> Windows { get; set; } = new List<Window>();
    }
}
