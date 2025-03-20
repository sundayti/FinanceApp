using Application.Facades;

namespace Application.Commands;

public class RenameBankAccountCommand : ICommand
{
    private readonly BankAccountFacade _facade;
    private readonly Guid _accountId;
    private readonly string _newName;

    public RenameBankAccountCommand(BankAccountFacade facade, Guid accountId, string newName)
    {
        _facade = facade;
        _accountId = accountId;
        _newName = newName;
    }

    public void Execute()
    {
        _facade.RenameBankAccount(_accountId, _newName);
        Console.WriteLine("Счёт успешно переименован.");
    }
}