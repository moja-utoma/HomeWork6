using System.ComponentModel.DataAnnotations;

public class Category
{
    public int ID { get; set; }
    [Required]
    public required string Name { get; set; }
    public List<Expense> Expenses { get; set; } = new();
}