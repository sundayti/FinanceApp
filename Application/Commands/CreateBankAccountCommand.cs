using Application.Facades;

namespace Application.Commands;

public class CreateBankAccountCommand : ICommand
{
    private readonly BankAccountFacade _facade;
    private readonly string _accountName;
    private readonly decimal _initialBalance;

    public CreateBankAccountCommand(
        BankAccountFacade facade,
        string accountName,
        decimal initialBalance)
    {
        _facade = facade;
        _accountName = accountName;
        _initialBalance = initialBalance;
    }

    public void Execute()
    {
        var account = _facade.CreateBankAccount(_accountName, _initialBalance);
        Console.WriteLine($"Создан новый счёт: {account.Name}, баланс={account.Balance}");
    }
}