using Domain.Entities;
using Domain.Interfaces;
using Application.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly FinanceDbContext _context;

    public CategoryRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public Category? GetById(Guid id)
    {
        return _context.Categories.Find(id);
    }

    public IEnumerable<Category> GetAll()
    {
        return _context.Categories.AsNoTracking().ToList();
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var cat = _context.Categories.Find(id);
        if (cat != null)
        {
            _context.Categories.Remove(cat);
            _context.SaveChanges();
        }
    }
}
