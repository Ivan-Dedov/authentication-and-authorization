using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MyWeatherApp;

public sealed class WeatherAuthenticationOptions : AuthenticationSchemeOptions;

public sealed class WeatherAuthenticationHandler : AuthenticationHandler<WeatherAuthenticationOptions>
{
    public WeatherAuthenticationHandler(IOptionsMonitor<WeatherAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    // ВНИМАНИЕ!
    // Код сделан только для демонстрации!
    // В Production следует использовать стандартные механизмы аутентификации (JWT, OAuth, Cookie или другие)! 
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var weatherAuth = Context.Request.Headers[AuthenticationConstants.WeatherAuthenticationHeader];

        if (weatherAuth.Count != 1)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        // Считаем, что в хэдере должна быть строка, в которой через запятую указана страна и возраст
        var parts = weatherAuth.ToString().Split(",");
        if (parts.Length != 2)
        {
            return Task.FromResult(AuthenticateResult.Fail("Malformed header"));
        }

        var country = parts[0];
        var age = parts[1];

        var claims = new[]
        {
            new Claim(ClaimTypes.Country, country),
            new Claim(WeatherClaims.Age, age),
        };

        // Обязательно передаем AuthenticationType - иначе User.IsAuthenticated будет false!
        var identity = new ClaimsIdentity(claims, AuthenticationConstants.WeatherAuthenticationType);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationConstants.WeatherAuthenticationScheme);
        var success = AuthenticateResult.Success(ticket);
        return Task.FromResult(success);
    }
}
