using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(
                dest => dest.TotalExpensesLastMonth,
                opt =>
                    opt.MapFrom(src =>
                        src.Expenses.Where(e => e.DateOfExpense >= DateTime.Now.AddMonths(-1))
                            .Sum(e => (decimal?)e.Sum) ?? 0m
                    )
            )
            .ForMember(
                dest => dest.RecentExpenses,
                opt =>
                    opt.MapFrom(src =>
                        src.Expenses.Where(e => e.DateOfExpense >= DateTime.Now.AddMonths(-1))
                            .OrderByDescending(e => e.DateOfExpense)
                    )
            );

        CreateMap<CategoryDto, Category>().ForMember(dest => dest.Expenses, opt => opt.Ignore());

        CreateMap<Expense, ExpenseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<ExpenseDto, Expense>();

        CreateMap<Expense, MonthlyExpenseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID));
    }
}
