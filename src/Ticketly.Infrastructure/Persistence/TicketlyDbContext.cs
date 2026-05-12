using Microsoft.EntityFrameworkCore;
using Ticketly.Domain.Entities;

namespace Ticketly.Infrastructure.Persistence;

public sealed class TicketlyDbContext(DbContextOptions<TicketlyDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events => Set<Event>();

    public DbSet<TicketType> TicketTypes => Set<TicketType>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("events");
            entity.HasKey(ticketlyEvent => ticketlyEvent.Id);

            entity.Property(ticketlyEvent => ticketlyEvent.Id).ValueGeneratedNever();
            entity.Property(ticketlyEvent => ticketlyEvent.Name).HasMaxLength(200).IsRequired();
            entity.Property(ticketlyEvent => ticketlyEvent.Venue).HasMaxLength(200).IsRequired();
            entity.Property(ticketlyEvent => ticketlyEvent.StartsAt).IsRequired();
            entity.Property(ticketlyEvent => ticketlyEvent.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.ToTable("ticket_types");
            entity.HasKey(ticketType => ticketType.Id);

            entity.Property(ticketType => ticketType.Id).ValueGeneratedNever();
            entity.Property(ticketType => ticketType.Name).HasMaxLength(200).IsRequired();
            entity.Property(ticketType => ticketType.Price).HasPrecision(18, 2).IsRequired();
            entity.Property(ticketType => ticketType.Currency).HasMaxLength(3).IsRequired();
            entity.Property(ticketType => ticketType.TotalQuantity).IsRequired();
            entity.Property(ticketType => ticketType.AvailableQuantity).IsRequired();
            entity.Property(ticketType => ticketType.CreatedAt).IsRequired();

            entity.HasOne<Event>()
                .WithMany()
                .HasForeignKey(ticketType => ticketType.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(ticketType => ticketType.EventId);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("reservations");
            entity.HasKey(reservation => reservation.Id);

            entity.Property(reservation => reservation.Id).ValueGeneratedNever();
            entity.Property(reservation => reservation.Quantity).IsRequired();
            entity.Property(reservation => reservation.CustomerEmail).HasMaxLength(320).IsRequired();
            entity.Property(reservation => reservation.CreatedAt).IsRequired();

            entity.HasOne<TicketType>()
                .WithMany()
                .HasForeignKey(reservation => reservation.TicketTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(reservation => reservation.TicketTypeId);
        });
    }
}
