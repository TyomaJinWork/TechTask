using Domain.DTO;
using CSharpFunctionalExtensions;

namespace Application.Services
{
    public interface IWindowsService
    {
        Task<Result<bool>> AddWindowsForProduct(int singleProductId, int comboProductId, List<CreateWindowDTO> windows);
        Task<Result<InfoResultDTO>> GetInfoByProductNameAndDateRange(string productName, string from, string to);
    }
}