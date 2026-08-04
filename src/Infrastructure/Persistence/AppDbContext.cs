using Microsoft.EntityFrameworkCore;
using VidirDashboard.Domain.Entities;

namespace VidirDashboard.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<Bin> Bins => Set<Bin>();
    public DbSet<MoveCommand> MoveCommands => Set<MoveCommand>();
    public DbSet<FaultEvent> FaultEvents => Set<FaultEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Machine>()
            .HasMany(m => m.Bins)
            .WithOne(b => b.Machine!)
            .HasForeignKey(b => b.MachineId);

        modelBuilder.Entity<Machine>()
            .HasMany(m => m.MoveCommands)
            .WithOne(c => c.Machine!)
            .HasForeignKey(c => c.MachineId);

        modelBuilder.Entity<Machine>()
            .HasMany(m => m.FaultEvents)
            .WithOne(f => f.Machine!)
            .HasForeignKey(f => f.MachineId);

        modelBuilder.Entity<Bin>()
            .HasIndex(b => new { b.MachineId, b.Position })
            .IsUnique();
    }
}