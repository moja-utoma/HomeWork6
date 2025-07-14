using System.ComponentModel.DataAnnotations;

public class Expense
{
    public int ID { get; set; }

    [Required]
    public decimal Sum { get; set; }
    public required string Comment { get; set; }

    public int CategoryID { get; set; }
    public required Category Category { get; set; }
}