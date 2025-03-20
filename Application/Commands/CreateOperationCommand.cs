using Application.Facades;

namespace Application.Commands.Decorators;

public class CreateOperationCommand : ICommand
{
    private readonly FinancialModuleFacade _facade;
    private readonly Guid _accountId;
    private readonly Guid _categoryId;
    private readonly decimal _amount;
    private readonly DateTime _date;
    private readonly string _description;

    public CreateOperationCommand(
        FinancialModuleFacade facade,
        Guid accountId,
        Guid categoryId,
        decimal amount,
        DateTime date,
        string description)
    {
        _facade = facade;
        _accountId = accountId;
        _categoryId = categoryId;
        _amount = amount;
        _date = date;
        _description = description;
    }

    public void Execute()
    {
        _facade.CreateOperation(_accountId, _categoryId, _amount, _date, _description);
    }
}