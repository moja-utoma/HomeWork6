using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Expense
{
    public int ID { get; set; }

    [Required]
    public decimal Sum { get; set; }
    public required string Comment { get; set; }

    [Column(TypeName = "timestamp(6)")]
    public DateTime DateOfExpense { get; set; } = DateTime.Now;

    public int CategoryID { get; set; }
    public required Category Category { get; set; }
}
