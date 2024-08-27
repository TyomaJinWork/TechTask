using API.Contracts;
using Application.Services;
using CSharpFunctionalExtensions;
using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class MainController : ControllerBase
    {
        private IWindowsService _windowsService;
        private IPricesService _pricesService;

        public MainController(IWindowsService windowsService, IPricesService pricesService)
        {
            _windowsService = windowsService;
            _pricesService = pricesService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<bool>>> AddWindows([FromBody] CreateWindowForProductRequest request)
        {
            var createWindowsResult = await _windowsService.AddWindowsForProduct(request.singleProductId, request.comboProductId, request.Windows);

            var result = new Response<bool>(createWindowsResult);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Response<bool>>> ChangePrice([FromBody] ChangePriceRequest request)
        {
            var changePriceResult = await _pricesService.ChangePriceAsync(request.WindowId, request.TimeIntervalId, request.Price);

            var result = new Response<bool>(changePriceResult);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<Response<InfoResultDTO>>> GetInfo(string productName, string from, string to)
        {
            var infoResult = await _windowsService.GetInfoByProductNameAndDateRange(productName, from, to);

            var result = new Response<InfoResultDTO>(infoResult);

            return Ok(result);
        }
    }
}
