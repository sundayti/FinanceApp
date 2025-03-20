using Domain.Entities;
using Domain.Factories;
using Domain.Interfaces;

namespace Application.Facades;

public class BankAccountFacade
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly FinanceFactory _factory;

    public BankAccountFacade(IBankAccountRepository bankAccountRepository, FinanceFactory factory)
    {
        _bankAccountRepository = bankAccountRepository;
        _factory = factory;
    }

    public BankAccount CreateBankAccount(string name, decimal initialBalance)
    {
        var account = _factory.CreateBankAccount(name, initialBalance);
        _bankAccountRepository.Add(account);
        return account;
    }

    public void RenameBankAccount(Guid id, string newName)
    {
        var account = _bankAccountRepository.GetById(id);
        if (account == null) return;
        account.Rename(newName);
        _bankAccountRepository.Update(account);
    }

    public void Deposit(Guid accountId, decimal amount)
    {
        var account = _bankAccountRepository.GetById(accountId);
        if (account == null) return;
        account.Deposit(amount);
        _bankAccountRepository.Update(account);
    }

    public void Withdraw(Guid accountId, decimal amount)
    {
        var account = _bankAccountRepository.GetById(accountId);
        if (account == null) return;
        account.Withdraw(amount);
        _bankAccountRepository.Update(account);
    }

    public IEnumerable<BankAccount> GetAllAccounts()
    {
        return _bankAccountRepository.GetAll();
    }

    public void DeleteAccount(Guid id)
    {
        _bankAccountRepository.Delete(id);
    }

    public void UpsertAccount(BankAccount account)
    {
        var existing = _bankAccountRepository.GetById(account.Id);
        if (existing == null)
        {
            _bankAccountRepository.Add(account);
        }
        else
        {
            existing.SetName(account.Name);
            existing.SetBalance(account.Balance);
            _bankAccountRepository.Update(existing);
        }
    }
}