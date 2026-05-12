using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ticketly.Infrastructure.Persistence;

public sealed class TicketlyDbContextFactory : IDesignTimeDbContextFactory<TicketlyDbContext>
{
    public TicketlyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TicketlyDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=ticketly;Username=ticketly";

        optionsBuilder.UseNpgsql(connectionString);

        return new TicketlyDbContext(optionsBuilder.Options);
    }
}
