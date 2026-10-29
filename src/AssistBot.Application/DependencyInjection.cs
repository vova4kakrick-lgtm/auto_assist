using AssistBot.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace AssistBot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        return services;
    }
}
