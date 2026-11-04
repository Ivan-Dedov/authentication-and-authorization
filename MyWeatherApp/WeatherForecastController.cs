using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyWeatherApp;

[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastController : ControllerBase
{
    [HttpGet("{city}")]
    [Authorize(Policy = AuthorizationConstants.RussianOver18Policy)]
    public WeatherForecast Get([FromRoute] string city)
    {
        return new WeatherForecast
        {
            City = city,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Temperature = Random.Shared.Next(-20, 55),
        };
    }
}

public readonly record struct WeatherForecast
{
    public required string City { get; init; }

    public required DateOnly Date { get; init; }

    public required int Temperature { get; init; }
}
