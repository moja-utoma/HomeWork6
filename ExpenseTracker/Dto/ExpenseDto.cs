public class ExpenseDto
{
    public int ID { get; set; }
    public decimal Sum { get; set; }
    public string Comment { get; set; } = default!;
    public DateTime DateOfExpense { get; set; }

    public int CategoryID { get; set; }
    public string? CategoryName { get; set; } = default!;
}
