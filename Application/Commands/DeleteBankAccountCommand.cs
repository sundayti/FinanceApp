using Application.Facades;

namespace Application.Commands;

public class DeleteBankAccountCommand : ICommand
{
    private readonly BankAccountFacade _facade;
    private readonly Guid _accountId;

    public DeleteBankAccountCommand(BankAccountFacade facade, Guid accountId)
    {
        _facade = facade;
        _accountId = accountId;
    }

    public void Execute()
    {
        _facade.DeleteAccount(_accountId);
        Console.WriteLine("Счёт успешно удалён.");
    }
}