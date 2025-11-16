using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace Core.Data;

public class StoreDbContext: DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.ProductId);

            entity.Property(p => p.Name)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(p => p.Description)
                  .HasMaxLength(500);

            entity.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)");

            entity.Property(p => p.MinPrice)
                  .HasColumnType("decimal(18,2)");

            entity.Property(p => p.CreatedOn);

            entity.Property(p => p.UpdatedOn);
        });
    }
}
