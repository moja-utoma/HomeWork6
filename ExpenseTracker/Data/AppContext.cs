using Microsoft.EntityFrameworkCore;

public class AppContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    public AppContext(DbContextOptions<AppContext> options)
      : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.ID);
            entity.Property(c => c.ID).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(exp => exp.ID);
            entity.HasOne(exp => exp.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(exp => exp.CategoryID);
        });
    }
}