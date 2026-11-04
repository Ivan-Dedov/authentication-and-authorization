using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MyWeatherApp;

/// <summary>
/// Требование к минимальному возрасту
/// </summary>
public sealed class AgeRequirement : IAuthorizationRequirement
{
    public required int MinAge { get; init; }
}

/// <summary>
/// Обработчик требования <see cref="AgeRequirement"/>
/// </summary>
public sealed class AgeRequirementHandler : AuthorizationHandler<AgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AgeRequirement requirement)
    {
        var ageClaim = context.User.FindFirstValue(WeatherClaims.Age);

        if (int.TryParse(ageClaim, out var age) && age > requirement.MinAge)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new AuthorizationFailureReason(this, $"User is not over {requirement.MinAge} years old"));
        }

        return Task.CompletedTask;
    }
}
