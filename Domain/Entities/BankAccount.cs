using System.Text.Json.Serialization;
using Domain.Interfaces;

namespace Domain.Entities;

public class BankAccount : IVisitable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Balance { get; private set; }

    protected BankAccount() { }

    internal BankAccount(Guid id, string name)
    {
        Id = id;
        Name = name;
        Balance = 0;
    }
    
    [JsonConstructor]
    internal BankAccount(Guid id, string name, decimal balance)
    {
        Id = id;
        Name = name;
        Balance = balance;
    }

    public void Deposit(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Сумма не может быть отрицательной.");
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Сумма не может быть отрицательной.");
        Balance -= amount;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Название не может быть пустым.");
        Name = newName;
    }
    
    public void SetBalance(decimal newBalance)
    {
        Balance = newBalance;
    }

    public void SetName(string newName)
    {
        Name = newName;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
