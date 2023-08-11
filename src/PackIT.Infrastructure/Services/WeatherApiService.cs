using PackIT.Application.DTO.External;
using PackIT.Application.Services;
using PackIT.Domain.ValueObjects;

namespace PackIT.Infrastructure.Services
{
    internal sealed class WeatherApiService : IWeatherApiService
    {
        public Task<WeatherDto> GetWeatherAsync(Localization localization)
        {
            // Simulate external call
            return Task.FromResult(new WeatherDto(new Random().Next(5, 30)));
        }
    }
}
