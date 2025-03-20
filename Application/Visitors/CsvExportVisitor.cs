using System.Globalization;
using System.Text;
using CsvHelper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Visitors;

public class CsvExportVisitor : IVisitor
{
    private readonly List<BankAccount> _accounts = new();
        
    public void Visit(BankAccount bankAccount)
    {
        _accounts.Add(bankAccount);
    }

    public void Visit(Category category) { }

    public void Visit(Operation operation) { }

    public string GetResult()
    {
        using (var writer = new StringWriter(new StringBuilder()))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(_accounts);
            return writer.ToString();
        }
    }
}