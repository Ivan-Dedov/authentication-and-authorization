namespace MyWeatherApp;

public static class AuthorizationConstants
{
    /// <summary>
    /// Наименование политики, проверяющей страну == "Russia" или "Ru" и возраст > 18
    /// </summary>
    public const string RussianOver18Policy = "RussianOver18";
}
