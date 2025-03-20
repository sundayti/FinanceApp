using Domain.Entities;
using Domain.Interfaces;

namespace Application.Proxy;

public class BankAccountRepositoryProxy : IBankAccountRepository
{
    private readonly IBankAccountRepository _realRepository;
    private readonly Dictionary<Guid, BankAccount> _cache = new();

    public BankAccountRepositoryProxy(IBankAccountRepository realRepository)
    {
        _realRepository = realRepository;
        LoadCache();
    }

    public void Add(BankAccount account)
    {
        _realRepository.Add(account);
        _cache[account.Id] = account;
    }

    public BankAccount? GetById(Guid id)
    {
        if (_cache.TryGetValue(id, out var cached))
            return cached;

        var fromDb = _realRepository.GetById(id);
        if (fromDb != null)
            _cache[fromDb.Id] = fromDb;

        return fromDb;
    }

    public IEnumerable<BankAccount> GetAll()
    {
        return _cache.Values;
    }

    public void Update(BankAccount account)
    {
        _realRepository.Update(account);
        _cache[account.Id] = account;
    }

    public void Delete(Guid id)
    {
        _realRepository.Delete(id);
        _cache.Remove(id);
    }

    private void LoadCache()
    {
        var allAccounts = _realRepository.GetAll();
        foreach (var acc in allAccounts)
        {
            _cache[acc.Id] = acc;
        }
    }
}