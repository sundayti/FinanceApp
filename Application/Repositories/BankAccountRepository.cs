using Application.Infrastructure;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly FinanceDbContext _context;

    public BankAccountRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public void Add(BankAccount account)
    {
        _context.BankAccounts.Add(account);
        _context.SaveChanges();
    }

    public BankAccount? GetById(Guid id)
    {
        return _context.BankAccounts.Find(id);
    }

    public IEnumerable<BankAccount> GetAll()
    {
        return _context.BankAccounts.AsNoTracking().ToList();
    }

    public void Update(BankAccount account)
    {
        _context.BankAccounts.Update(account);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var acc = _context.BankAccounts.Find(id);
        if (acc != null)
        {
            _context.BankAccounts.Remove(acc);
            _context.SaveChanges();
        }
    }
}
