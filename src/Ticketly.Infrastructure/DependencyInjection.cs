using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketly.Application.Abstractions;
using Ticketly.Infrastructure.Persistence;

namespace Ticketly.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        services.AddDbContext<TicketlyDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ITicketlyRepository, TicketlyRepository>();

        return services;
    }
}
