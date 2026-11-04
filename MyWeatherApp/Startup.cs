using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Scalar.AspNetCore;

namespace MyWeatherApp;

public sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Подключаем базовые зависимости для реализации API
        services.AddControllers();
        // Нужно для генерации файла спецификации API, чтобы отобразить эндпоинты в UI Scalar
        services.AddOpenApi();

        // Подключаем зависимости для middleware аутентификации и авторизации:

        services
            .AddAuthentication()
            // Добавляем нашу кастомную схему аутентификации
            .AddScheme<WeatherAuthenticationOptions, WeatherAuthenticationHandler>(
                AuthenticationConstants.WeatherAuthenticationScheme,
                static configure =>
                {
                    // В кастомной схеме можно не заполнять свойства - для всего используем реализации по умолчанию.
                });

        services.AddAuthorization(static builder =>
        {
            // Создаём кастомную политику.
            builder.AddPolicy(
                AuthorizationConstants.RussianOver18Policy,
                static policy =>
                {
                    policy
                        .AddRequirements(new AgeRequirement { MinAge = 18 })
                        .AddRequirements(new ClaimsAuthorizationRequirement(ClaimTypes.Country, ["Ru", "Russia"]));
                });
        });
        services.TryAddEnumerable(ServiceDescriptor.Transient<IAuthorizationHandler, AgeRequirementHandler>());
    }

    public void Configure(IApplicationBuilder app)
    {
        // Используем middleware, отвечающую за сопоставление запроса и эндпоинта
        app.UseRouting();

        /*
         * Здесь можно регистрировать middleware, которые не зависят от HttpContext пользователя
         * и не полагаются на наличие у него каких-либо ролей.
         */

        // Сначала выполняется аутентификация, затем – авторизация!
        app.UseAuthentication();
        app.UseAuthorization();

        /*
         * Прочие middleware, которые обращаются к контексту пользователя или ожидают,
         * что пользователь аутентифицирован и авторизован, должны быть зарегистрированы в этом блоке.
         */
        
        // Middleware, вызывающая код найденного эндпоинта
        app.UseEndpoints(static builder =>
        {
            builder.MapControllers();

            // Подключаем UI Scalar
            builder.MapOpenApi();
            builder.MapScalarApiReference();
        });
    }
}
