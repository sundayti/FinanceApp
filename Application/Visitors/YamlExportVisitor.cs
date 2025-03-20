using Application.DTO;
using Domain.Entities;
using Domain.Interfaces;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Application.Visitors;

public class YamlExportVisitor : IVisitor
{
    private readonly DataTransferDto _dto = new ();

    public void Visit(BankAccount bankAccount)
    {
        _dto.BankAccounts.Add(bankAccount);
    }

    public void Visit(Category category)
    {
        _dto.Categories.Add(category);
    }

    public void Visit(Operation operation)
    {
        _dto.Operations.Add(operation);
    }

    public string GetResult()
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return serializer.Serialize(_dto);
    }
}