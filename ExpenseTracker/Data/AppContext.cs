using Microsoft.EntityFrameworkCore;

public class AppContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    public AppContext(DbContextOptions<AppContext> options)
      : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.ID);
            entity.Property(c => c.ID).ValueGeneratedOnAdd();
            entity.HasData(
                new Category { ID = 1, Name = "Продукти харчування" },
                new Category { ID = 2, Name = "Транспорт" },
                new Category { ID = 3, Name = "Мобільний зв'язок" },
                new Category { ID = 4, Name = "Інтернет" },
                new Category { ID = 5, Name = "Розваги" }
            );
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(exp => exp.ID);
            entity.HasOne(exp => exp.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(exp => exp.CategoryID)
            .OnDelete(DeleteBehavior.Cascade);
        });
    }
}