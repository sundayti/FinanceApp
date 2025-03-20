using Domain.Entities;
using Domain.Interfaces;
using Application.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class OperationRepository : IOperationRepository
{
    private readonly FinanceDbContext _context;

    public OperationRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public void Add(Operation operation)
    {
        _context.Operations.Add(operation);
        _context.SaveChanges();
    }

    public Operation? GetById(Guid id)
    {
        return _context.Operations.Find(id);
    }

    public IEnumerable<Operation> GetAll()
    {
        return _context.Operations.AsNoTracking().ToList();
    }

    public void Update(Operation operation)
    {
        _context.Operations.Update(operation);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var op = _context.Operations.Find(id);
        if (op != null)
        {
            _context.Operations.Remove(op);
            _context.SaveChanges();
        }
    }
}