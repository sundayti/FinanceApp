using Domain.Entities;

namespace Domain.Interfaces;

public interface IVisitor
{
    void Visit(BankAccount account);
    void Visit(Category category);
    void Visit(Operation operation);
    string GetResult();
}