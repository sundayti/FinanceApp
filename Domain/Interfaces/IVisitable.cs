namespace Domain.Interfaces;

public interface IVisitable
{
    void Accept(IVisitor visitor);
}