namespace MyWeatherApp;

public static class AuthenticationConstants
{
    /// <summary>
    /// Схема аутентификации для приложения
    /// </summary>
    public const string WeatherAuthenticationScheme = "WeatherAuthentication";
    /// <summary>
    /// Тип аутентификации для приложения
    /// </summary>
    public const string WeatherAuthenticationType = nameof(WeatherAuthenticationType);

    /// <summary>
    /// Хэдер для получения credentials
    /// </summary>
    public const string WeatherAuthenticationHeader = "weather-auth";
}
