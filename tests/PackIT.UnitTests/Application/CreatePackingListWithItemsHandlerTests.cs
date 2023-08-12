using PackIT.Shared.Abstractions.Commands;
using PackIT.Application.Commands;
using PackIT.Domain.Repositories;
using PackIT.Application.Services;
using PackIT.Domain.Factories;
using NSubstitute;
using PackIT.Application.Commands.Handlers;
using PackIT.Domain.Consts;
using Shouldly;
using PackIT.Application.Exceptions;
using PackIT.Domain.ValueObjects;
using PackIT.Application.DTO.External;
using PackIT.Domain.Entities;

namespace PackIT.UnitTests.Application
{
    public class CreatePackingListWithItemsHandlerTests
    {
        private readonly ICommandHandler<CreatePackingListWithItems> _commandHandler;
        private readonly IPackingListRepository _repository;
        private readonly IWeatherApiService _weatherService;
        private readonly IPackingListReadService _readService;
        private readonly IPackingListFactory _factory;

        public CreatePackingListWithItemsHandlerTests()
        {
            _repository = Substitute.For<IPackingListRepository>();
            _weatherService = Substitute.For<IWeatherApiService>();
            _readService = Substitute.For<IPackingListReadService>();
            _factory = Substitute.For<IPackingListFactory>();

            _commandHandler = new CreatePackingListWithItemsHandler(_readService, _factory, _repository, _weatherService);
        }

        Task Act(CreatePackingListWithItems command)
            => _commandHandler.HandleAsync(command);

        [Fact]
        public async Task HandleAsync_Throws_PackingListAlreadyExistsException_When_List_With_Same_Name_Already_Exists()
        {
            // Arrange
            var command = new CreatePackingListWithItems(Guid.NewGuid(), "MyList", 10, Gender.Female, default);
            _readService.ExistsByNameAsync(command.Name).Returns(true);

            // Act
            var exception = await Record.ExceptionAsync(() => Act(command));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<PackingListAlreadyExistsException>();
        }

        [Fact]
        public async Task HandleAsync_Throws_LocalizationMissingException_When_Weather_Is_Not_Returned_From_Service()
        {
            // Arrange
            var command = new CreatePackingListWithItems(Guid.NewGuid(), "MyList", 10, 
                Gender.Female, new LocalizationWriteModel("Warsaw", "Poland"));

            _readService.ExistsByNameAsync(command.Name).Returns(false);
            _weatherService.GetWeatherAsync(Arg.Any<Localization>()).Returns(default(WeatherDto));

            // Act
            var exception  = await Record.ExceptionAsync(() => Act(command));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<MissingLocalizationWeatherException>();
        }

        [Fact]
        public async Task HandleAsync_Calls_Repository_On_Success()
        {
            // Arrange
            var command = new CreatePackingListWithItems(Guid.NewGuid(), "MyList", 10,
                Gender.Female, new LocalizationWriteModel("Warsaw", "Poland"));

            _readService.ExistsByNameAsync(command.Name).Returns(false);
            _weatherService.GetWeatherAsync(Arg.Any<Localization>()).Returns(new WeatherDto(12));
            _factory.CreateWithDefaultItems(command.Id, command.Name, command.Days, command.Gender, Arg.Any<Localization>(), Arg.Any<Temperature>())
                .Returns(default(PackingList));

            // Act
            await Act(command);

            // Assert
            await _repository.Received(1).AddAsync(Arg.Any<PackingList>());
        }
    }
}
