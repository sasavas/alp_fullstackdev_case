using ForceGetCase.Application.Models.WeatherForecast;

namespace ForceGetCase.Application.Services;

public interface IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecastResponseModel>> GetAsync();
}
