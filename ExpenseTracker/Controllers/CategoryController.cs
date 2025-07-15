using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppContext _context;
    private readonly IMapper _mapper;

    public CategoryController(AppContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Category
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.Include(c => c.Expenses).ToListAsync();

        var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
        return Ok(categoryDtos);
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _context
            .Categories.Include(c => c.Expenses)
            .FirstOrDefaultAsync(c => c.ID == id);

        if (category == null)
            return NotFound();

        var dto = _mapper.Map<CategoryDto>(category);
        return Ok(dto);
    }

    // POST: api/Category
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = _mapper.Map<Category>(categoryDto);

        var exists = await _context.Categories.AnyAsync(c =>
            c.Name.ToLower() == category.Name.ToLower()
        );
        if (exists)
            return BadRequest("Категорія з таким іменем вже існує.");

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var createdDto = _mapper.Map<CategoryDto>(category);
        return CreatedAtAction(nameof(GetById), new { id = category.ID }, createdDto);
    }

    // PUT: api/Category/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDto categoryDto)
    {
        if (id != categoryDto.ID)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory == null)
            return NotFound();

        _mapper.Map(categoryDto, existingCategory);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
