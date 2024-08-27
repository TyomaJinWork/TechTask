using Domain.DTO;
using CSharpFunctionalExtensions;
using Domain.Abstractions;
using Domain.Entities;
using System.Text;
using DataAccess;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class WindowsService : IWindowsService
    {
        private readonly ITimeIntervalsService _timeIntervalsService;
        private readonly IPricesService _pricesService;
        private readonly IPricesRepository _pricesRepository;
        private readonly IWindowsRepository _windowsRepository;
        private readonly ISingleProductsRepository _singleProductRepository;
        private readonly IComboProductsRepository _comboProductRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<WindowsService> _logger;

        public WindowsService(ITimeIntervalsService timeIntervalsService, IWindowsRepository windowsRepository,
            ISingleProductsRepository singleProductRepository, IComboProductsRepository comboProductRepository,
            IPricesService pricesService, AppDbContext context, IPricesRepository pricesRepository,
            ILogger<WindowsService> logger)
        {
            _timeIntervalsService = timeIntervalsService;
            _singleProductRepository = singleProductRepository;
            _comboProductRepository = comboProductRepository;
            _windowsRepository = windowsRepository;
            _pricesService = pricesService;
            _context = context;
            _pricesRepository = pricesRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> AddWindowsForProduct(int singleProductId, int comboProductId, List<CreateWindowDTO> windows)
        {
            var errorMessages = new StringBuilder();
            SingleProduct? singleProduct = null;
            ComboProduct? comboProduct = null;

            if (singleProductId == 0 && comboProductId == 0)
            {
                var message = "Product wasn't provided.";

                _logger.LogWarning("Fail: {Message}", message);
                return Result.Failure<bool>(message);
            }
            else if (singleProductId > 0 && comboProductId > 0)
            {
                var message = "You can choose only one product per request.";

                _logger.LogWarning("Fail: {Message}", message);
                return Result.Failure<bool>(message);
            }

            if (singleProductId != 0)
            {
                var singleProductResult = await _singleProductRepository.GetAsync(singleProductId);

                if (singleProductResult.IsFailure)
                {
                    _logger.LogWarning("Fail: {Message}", singleProductResult.Error);
                    return Result.Failure<bool>(singleProductResult.Error);
                }
                else
                {
                    singleProduct = singleProductResult.Value;
                }
            }
            else
            {
                var comboProductResult = await _comboProductRepository.GetAsync(comboProductId);

                if (comboProductResult.IsFailure)
                {
                    _logger.LogWarning("Fail: {Message}", comboProductResult.Error);
                    return Result.Failure<bool>(comboProductResult.Error);
                }
                else
                {
                    comboProduct = comboProductResult.Value;
                }
            }

            using var transaction = _context.Database.BeginTransaction();

            foreach (var windowItem in windows)
            {
                var createWindowResult = singleProduct != null
                    ? await CreateWindow(singleProduct, windowItem, windowItem.Price, null)
                    : await CreateWindow(comboProduct!, windowItem);

                if (createWindowResult.IsFailure)
                {
                    errorMessages.AppendLine(createWindowResult.Error + ";");
                    continue;
                }
            }

            if (errorMessages.Length > 0)
            {
                transaction.Rollback();

                _logger.LogWarning("Fail: {Message}", errorMessages.ToString());
                return Result.Failure<bool>(errorMessages.ToString());
            }

            transaction.Commit();

            return Result.Success(true);
        }

        private async Task<Result> CreateWindow(SingleProduct singleProduct, CreateWindowDTO window, decimal price, ComboProduct? comboProduct = null)
        {
            DateTime startDate;
            DateTime endDate;
            TimeOnly startTime;
            TimeOnly endTime;

            if (!DateTime.TryParse(window.WindowStartDate, out startDate))
            {
                return Result.Failure($"Invalid date format - {window.WindowStartDate}");
            }

            if (!DateTime.TryParse(window.WindowEndDate, out endDate))
            {
                return Result.Failure($"Invalid date format - {window.WindowEndDate}");
            }

            if (!TimeOnly.TryParse(window.IntervalStartTime, out startTime))
            {
                return Result.Failure($"Invalid time format - {window.IntervalStartTime}");
            }

            if (!TimeOnly.TryParse(window.IntervalEndTime, out endTime))
            {
                return Result.Failure($"Invalid time format - {window.IntervalEndTime}");
            }

            var isAvailableResult = await _pricesRepository.CheckAvailability(singleProduct,
                startDate, endDate, startTime.ToTimeSpan(), endTime.ToTimeSpan());

            if (isAvailableResult.IsFailure)
            {
                return Result.Failure(isAvailableResult.Error);
            }

            var windowEntity = new Window
            {
                Name = window.Name,
                StartDate = startDate,
                EndDate = endDate,
                SingleProduct = singleProduct,
                SingleProductId = singleProduct.SingleProductId,
                ComboProduct = comboProduct,
                ComboProductId = comboProduct != null ? comboProduct.ComboProductId : null,
            };

            var windowResult = await _windowsRepository.CreateAsync(windowEntity);

            if (windowResult.IsFailure)
            {
                return Result.Failure(windowResult.Error);
            }

            var timeIntervalsResult = await _timeIntervalsService.GenerateTimeIntervalsAsync(windowResult.Value, startTime, endTime);

            if (timeIntervalsResult.IsFailure)
            {
                return Result.Failure(timeIntervalsResult.Error);
            }

            var pricesResult = await _pricesService.AddPricesAsync(singleProduct, windowResult.Value, timeIntervalsResult.Value, price);

            if (pricesResult.IsFailure)
            {
                return Result.Failure(pricesResult.Error);
            }

            return Result.Success();
        }

        private async Task<Result> CreateWindow(ComboProduct comboProduct, CreateWindowDTO window)
        {
            var errorMessages = new StringBuilder();

            foreach (var singleProduct in comboProduct.SingleProducts)
            {
                decimal price = window.Price / comboProduct.SingleProducts.Count;
                var windowCreateResult = await CreateWindow(singleProduct, window, price, comboProduct);

                if (windowCreateResult.IsFailure)
                    errorMessages.AppendLine(windowCreateResult.Error + ";");
            }

            if (errorMessages.Length > 0)
                return Result.Failure(errorMessages.ToString());

            return Result.Success();
        }

        public async Task<Result<InfoResultDTO>> GetInfoByProductNameAndDateRange(string productName, string from, string to)
        {
            var infoResult = new InfoResultDTO();

            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParse(from, out startDate))
            {
                var message = $"Invalid date format - {from}";

                _logger.LogWarning("Fail: {Message}", message);
                return Result.Failure<InfoResultDTO>(message);
            }

            if (!DateTime.TryParse(to, out endDate))
            {
                var message = $"Invalid date format - {to}";

                _logger.LogWarning("Fail: {Message}", message);
                return Result.Failure<InfoResultDTO>(message);
            }

            var windowsResult = await _windowsRepository.GetByProductNameAndDateRange(productName, startDate, endDate);

            if (windowsResult.IsFailure)
            {
                _logger.LogWarning("Fail: {Message}", windowsResult.Error);
                return Result.Failure<InfoResultDTO>(windowsResult.Error);
            }

            if (windowsResult.Value.Any(x => x.ComboProduct != null))
            {
                var groups = windowsResult.Value.GroupBy(x => x.ComboProduct).ToList();

                foreach(var group in groups)
                {
                    var groupWindow = group.FirstOrDefault();

                    var window = new WindowResultDTO
                    {
                        StartDate = groupWindow!.StartDate.ToString(),
                        EndDate = groupWindow.EndDate.ToString(),
                        TimeIntervalPrices = groupWindow.TimeIntervals.Select(t => new TimeIntervalPriceResultDTO
                        {
                            StartTime = t.StartTime.ToString("c"),
                            EndTime = t.EndTime.ToString("c"),
                            Price = t.Price!.Value * groupWindow.ComboProduct!.SingleProducts.Count,
                        }).ToList(),
                    };

                    infoResult.Windows.Add(window);
                }
            }
            else
            {
                infoResult.Windows = windowsResult.Value.Select(w => new WindowResultDTO
                {
                    StartDate = w.StartDate.ToString(),
                    EndDate = w.EndDate.ToString(),
                    TimeIntervalPrices = w.TimeIntervals.Select(t => new TimeIntervalPriceResultDTO
                    {
                        StartTime = t.StartTime.ToString("c"),
                        EndTime = t.EndTime.ToString("c"),
                        Price = t.Price!.Value,
                    }).ToList(),
                }).ToList();
            }

            return Result.Success(infoResult);
        }
    }
}
