using Domain.Entities;
using Domain.Interfaces;

namespace Application.Proxy;

public class OperationRepositoryProxy : IOperationRepository
{
    private readonly IOperationRepository _realRepository;
    private readonly Dictionary<Guid, Operation> _cache = new();

    public OperationRepositoryProxy(IOperationRepository realRepository)
    {
        _realRepository = realRepository;
        LoadCache();
    }

    public void Add(Operation operation)
    {
        _realRepository.Add(operation);
        _cache[operation.Id] = operation;
    }

    public Operation? GetById(Guid id)
    {
        if (_cache.TryGetValue(id, out var cached))
            return cached;

        var fromDb = _realRepository.GetById(id);
        if (fromDb != null)
            _cache[fromDb.Id] = fromDb;
        return fromDb;
    }

    public IEnumerable<Operation> GetAll()
    {
        return _cache.Values;
    }

    public void Update(Operation operation)
    {
        _realRepository.Update(operation);
        _cache[operation.Id] = operation;
    }

    public void Delete(Guid id)
    {
        _realRepository.Delete(id);
        _cache.Remove(id);
    }

    private void LoadCache()
    {
        var allOperations = _realRepository.GetAll();
        foreach (var op in allOperations)
        {
            _cache[op.Id] = op;
        }
    }
}
