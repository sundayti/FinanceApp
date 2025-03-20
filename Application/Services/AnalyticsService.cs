using Application.Infrastructure;
using Domain.Entities;

namespace Application.Services;

public class AnalyticsService
{
    private readonly FinanceDbContext _context;

    public AnalyticsService(FinanceDbContext context)
    {
        _context = context;
    }
    
    public decimal CalculateIncomeExpenseDifference(DateTime start, DateTime end)
    {
        var operations = _context.Operations
            .Where(o => o.Date >= start && o.Date <= end);
        decimal totalIncome = operations
            .Where(o => o.Type == CategoryType.Income)
            .Sum(o => o.Amount);
        decimal totalExpense = operations
            .Where(o => o.Type == CategoryType.Expense)
            .Sum(o => o.Amount);
        return totalIncome - totalExpense;
    }
    
    public IEnumerable<OperationGroup> GroupOperationsByCategory(DateTime start, DateTime end)
    {
        var query = from op in _context.Operations
            where op.Date >= start && op.Date <= end
            join cat in _context.Categories on op.CategoryId equals cat.Id
            group op by cat.Name into g
            select new OperationGroup
            {
                CategoryName = g.Key,
                TotalAmount = g.Sum(op => op.Amount)
            };

        return query.ToList();
    }
}

public class OperationGroup
{
    public string CategoryName { get; set; }
    public decimal TotalAmount { get; set; }
}