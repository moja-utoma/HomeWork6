public class CategoryDto
{
    public int ID { get; set; }
    public string Name { get; set; } = default!;
    public decimal TotalExpensesLastMonth { get; set; }
    public List<ExpenseDto> RecentExpenses { get; set; } = new();
}
