using Domain.Entities;

namespace Domain.Factories;

public class FinanceFactory
{
    public BankAccount CreateBankAccount(string name, decimal initialBalance)
    {
        if (initialBalance < 0)
            throw new ArgumentException("Начальный баланс не может быть отрицательным.");

        return new BankAccount(Guid.NewGuid(), name, initialBalance);
    }

    public Category CreateCategory(string name, CategoryType type)
    {
        return new Category(Guid.NewGuid(), name, type);
    }

    public Operation CreateOperation(CategoryType type, Guid accountId, Guid categoryId,
        decimal amount, DateTime date, string? description)
    {
        if (amount < 0)
            throw new ArgumentException("Сумма операции не может быть отрицательной.");
            
        return new Operation(Guid.NewGuid(), type, accountId, categoryId, amount, date, description);
    }
}