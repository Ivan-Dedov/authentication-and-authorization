namespace MyWeatherApp;

public static class Program
{
    public static Task Main() => Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(static configure => configure.UseStartup<Startup>())
        .Build()
        .RunAsync();
}
