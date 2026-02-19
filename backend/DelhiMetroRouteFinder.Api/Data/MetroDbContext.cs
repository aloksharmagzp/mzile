using DelhiMetroRouteFinder.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DelhiMetroRouteFinder.Api.Data;

public class MetroDbContext(DbContextOptions<MetroDbContext> options) : DbContext(options)
{
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<Connection> Connections => Set<Connection>();
    public DbSet<FareRule> FareRules => Set<FareRule>();
    public DbSet<Line> Lines => Set<Line>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Station>()
            .HasOne(s => s.Line)
            .WithMany(l => l.Stations)
            .HasForeignKey(s => s.LineId);

        modelBuilder.Entity<Connection>()
            .HasOne(c => c.FromStation)
            .WithMany(s => s.OutgoingConnections)
            .HasForeignKey(c => c.FromStationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Connection>()
            .HasOne(c => c.ToStation)
            .WithMany(s => s.IncomingConnections)
            .HasForeignKey(c => c.ToStationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Connection>()
            .HasIndex(c => new { c.FromStationId, c.ToStationId })
            .IsUnique();

        modelBuilder.Entity<FareRule>()
            .HasIndex(f => new { f.MinStations, f.MaxStations })
            .IsUnique();
    }
}
