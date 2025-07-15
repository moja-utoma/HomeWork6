using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly AppContext _context;
    private readonly IMapper _mapper;

    public ExpenseController(AppContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Expense
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var expenses = await _context
            .Expenses.Include(e => e.Category)
            .OrderByDescending(e => e.DateOfExpense)
            .ToListAsync();

        var expenseDtos = _mapper.Map<List<ExpenseDto>>(expenses);
        return Ok(expenseDtos);
    }

    // GET: api/Expense/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var expense = await _context
            .Expenses.Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.ID == id);

        if (expense == null)
            return NotFound();

        var expenseDto = _mapper.Map<ExpenseDto>(expense);
        return Ok(expenseDto);
    }

    // POST: api/Expense
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpenseDto expenseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categoryExists = await _context.Categories.AnyAsync(c => c.ID == expenseDto.CategoryID);
        if (!categoryExists)
            return BadRequest("Категорія не існує");

        var expense = _mapper.Map<Expense>(expenseDto);

        var duplicate = await _context.Expenses.AnyAsync(e =>
            e.Sum == expense.Sum
            && e.Comment == expense.Comment
            && e.DateOfExpense == expense.DateOfExpense
            && e.CategoryID == expense.CategoryID
        );

        if (duplicate)
            return BadRequest("Така витрата вже існує.");

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        var createdDto = _mapper.Map<ExpenseDto>(expense);
        return CreatedAtAction(nameof(GetById), new { id = expense.ID }, createdDto);
    }

    // PUT: api/Expense/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ExpenseDto expenseDto)
    {
        if (id != expenseDto.ID)
            return BadRequest("ID не збігається");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingExpense = await _context.Expenses.FindAsync(id);
        if (existingExpense == null)
            return NotFound();

        var categoryExists = await _context.Categories.AnyAsync(c => c.ID == expenseDto.CategoryID);
        if (!categoryExists)
            return BadRequest("Категорія не існує");

        _mapper.Map(expenseDto, existingExpense);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Expense/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null)
            return NotFound();

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Expense/month
    [HttpGet("month")]
    public async Task<IActionResult> GetExpensesByMonth(
        [FromQuery] int? month,
        [FromQuery] int? year
    )
    {
        var now = DateTime.Now;

        int targetMonth = month ?? now.Month;
        int targetYear = year ?? now.Year;

        var fromDate = new DateTime(targetYear, targetMonth, 1);
        var toDate = fromDate.AddMonths(1);

        var expenses = await _context
            .Expenses.Where(e => e.DateOfExpense >= fromDate && e.DateOfExpense < toDate)
            .Include(e => e.Category)
            .OrderByDescending(e => e.DateOfExpense)
            .Select(e => new MonthlyExpenseDto
            {
                ID = e.ID,
                Sum = e.Sum,
                Comment = e.Comment,
                DateOfExpense = e.DateOfExpense,
                CategoryName = e.Category.Name,
            })
            .ToListAsync();

        return Ok(expenses);
    }
}
