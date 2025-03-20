using System.Globalization;
using Application.DTO;
using Application.Visitors;
using CsvHelper;
using Domain.Entities;

namespace Application.TemplateMethod;

public class CsvDataTransfer : DataTransferTemplate
{
    protected override string SerializeData(DataTransferDto data)
    {
        var csvVisitor = new CsvExportVisitor();
        foreach (var entity in data.BankAccounts)
        {
            entity.Accept(csvVisitor);
        }
        
        return csvVisitor.GetResult();
    }

    protected override DataTransferDto DeserializeData(string rawData)
    {
        using (var reader = new StringReader(rawData))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var accounts = csv.GetRecords<BankAccount>().ToList();
            return new DataTransferDto { BankAccounts = accounts };
        }
    }
}