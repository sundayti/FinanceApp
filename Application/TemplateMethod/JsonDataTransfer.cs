using System.Text.Encodings.Web;
using System.Text.Json;
using Application.DTO;
using Application.Visitors;

namespace Application.TemplateMethod;

public class JsonDataTransfer : DataTransferTemplate
{
    protected override string SerializeData(DataTransferDto data)
    {
        var jsonVisitor = new JsonExportVisitor();
        foreach (var entity in data.BankAccounts)
        {
            entity.Accept(jsonVisitor);
        }
        
        foreach (var entity in data.Operations)
        {
            entity.Accept(jsonVisitor);
        }
        
        foreach (var entity in data.Categories)
        {
            entity.Accept(jsonVisitor);
        }
        
        return jsonVisitor.GetResult();
    }

    protected override DataTransferDto DeserializeData(string rawData)
    {
        return JsonSerializer.Deserialize<DataTransferDto>(rawData);
    }
}