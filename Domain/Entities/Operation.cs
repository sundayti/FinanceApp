using System.Text.Json.Serialization;
using Domain.Interfaces;

namespace Domain.Entities;

public class Operation : IVisitable
{
    public Guid Id { get; private set; }
    public CategoryType Type { get; private set; }
    public Guid BankAccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string? Description { get; private set; }

    protected Operation() { }

    [JsonConstructor]
    internal Operation(
        Guid id,
        CategoryType type,
        Guid bankAccountId,
        Guid categoryId,
        decimal amount,
        DateTime date,
        string? description = null)
    {
        if (amount < 0)
            throw new ArgumentException("Сумма операции не может быть отрицательной.");

        Id = id;
        Type = type;
        BankAccountId = bankAccountId;
        CategoryId = categoryId;
        Amount = amount;
        Date = date;
        Description = description;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
