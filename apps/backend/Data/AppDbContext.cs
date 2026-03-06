using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Aircraft> Aircraft { get; set; } = null!;
    public DbSet<ComplianceLog> ComplianceLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Aircraft>()
            .HasMany(a => a.ComplianceLogs)
            .WithOne(cl => cl.Aircraft)
            .HasForeignKey(cl => cl.AircraftId);
    }
}
