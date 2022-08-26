using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Sack> Sacks => Set<Sack>();
    public DbSet<Package> Packages => Set<Package>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sack>().HasKey(s => s.Barcode);
        modelBuilder.Entity<Package>().HasKey(p => p.Barcode);
        modelBuilder.Entity<Package>().HasOne(p => p.Sack).WithMany(s => s.Packages);
    }
}
