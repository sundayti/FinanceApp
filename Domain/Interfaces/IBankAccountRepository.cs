using Domain.Entities;

namespace Domain.Interfaces;

public interface IBankAccountRepository
{
    void Add(BankAccount account);
    BankAccount? GetById(Guid id);
    IEnumerable<BankAccount> GetAll();
    void Update(BankAccount account);
    void Delete(Guid id);
}