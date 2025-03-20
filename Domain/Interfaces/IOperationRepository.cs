using Domain.Entities;

namespace Domain.Interfaces;

public interface IOperationRepository
{
    void Add(Operation operation);
    Operation? GetById(Guid id);
    IEnumerable<Operation> GetAll();
    void Update(Operation operation);
    void Delete(Guid id);
}