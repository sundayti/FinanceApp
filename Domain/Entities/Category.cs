using System.Text.Json.Serialization;
using Domain.Interfaces;

namespace Domain.Entities;

public enum CategoryType
{
    Income,
    Expense
}

public class Category : IVisitable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CategoryType Type { get; private set; }

    protected Category() { }

    [JsonConstructor]
    internal Category(Guid id, string name, CategoryType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Название категории не может быть пустым.");
        Name = newName;
    }

    public void ChangeType(CategoryType newType)
    {
        Type = newType;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public void SetName(string newName)
    {
        Name = newName;
    }
}
