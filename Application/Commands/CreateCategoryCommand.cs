using Application.Facades;
using Domain.Entities;

namespace Application.Commands;

public class CreateCategoryCommand : ICommand
{
    private readonly FinancialModuleFacade _facade;
    private readonly string _name;
    private readonly CategoryType _type;

    public CreateCategoryCommand(FinancialModuleFacade facade, string name, CategoryType type)
    {
        _facade = facade;
        _name = name;
        _type = type;
    }

    public void Execute()
    {
        _facade.CreateCategory(_name, _type);
        Console.WriteLine($"Категория '{_name}' успешно создана.");
    }
}