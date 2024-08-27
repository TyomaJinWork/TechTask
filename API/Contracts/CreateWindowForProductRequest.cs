using Domain.DTO;

namespace API.Contracts
{
    public class CreateWindowForProductRequest
    {
        public int comboProductId { get; set; } = default;
        public int singleProductId { get; set; } = default;
        public List<CreateWindowDTO> Windows { get; set; } = new List<CreateWindowDTO>();
    }
}
