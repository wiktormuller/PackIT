using PackIT.Application.Exceptions;
using PackIT.Application.Services;
using PackIT.Domain.Factories;
using PackIT.Domain.Repositories;
using PackIT.Domain.ValueObjects;
using PackIT.Shared.Abstractions.Commands;

namespace PackIT.Application.Commands.Handlers
{
    public sealed class CreatePackingListWithItemsHandler : ICommandHandler<CreatePackingListWithItems>
    {
        private readonly IPackingListReadService _packingReadService;
        private readonly IPackingListFactory _packingListFactory;
        private readonly IPackingListRepository _packingListRepository;
        private readonly IWeatherApiService _weatherApiService;

        public CreatePackingListWithItemsHandler(IPackingListReadService packingReadService,
            IPackingListFactory packingListFactory,
            IPackingListRepository packingListRepository,
            IWeatherApiService weatherApiService)
        {
            _packingReadService = packingReadService;
            _packingListFactory = packingListFactory;
            _packingListRepository = packingListRepository;
            _weatherApiService = weatherApiService;
        }

        public async Task HandleAsync(CreatePackingListWithItems command)
        {
            var (id, name, days, gender, localizationWriteModel) = command; // Records by default implement deconstructors

            // Idempotency based on name. Idempotency by Id is handled by storage
            if (await _packingReadService.ExistsByNameAsync(name)) // So, in this case, the name is our natural key
            {
                throw new PackingListAlreadyExistsException(name);
            }

            var localization = new Localization(localizationWriteModel.City, localizationWriteModel.Country);
            var weather = await _weatherApiService.GetWeatherAsync(localization);

            if (weather is null) // Business decision
            {
                throw new MissingLocalizationWeatherException(localization);
            }

            var packingList = _packingListFactory.CreateWithDefaultItems(id, name, days, gender, localization, weather.Temperature);

            await _packingListRepository.AddAsync(packingList);
        }
    }
}
