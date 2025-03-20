using Application.DTO;
using Application.Visitors;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Application.TemplateMethod;

public class YamlDataTransfer : DataTransferTemplate
{
    protected override string SerializeData(DataTransferDto data)
    {
        var yamlVisitor = new YamlExportVisitor();
        foreach (var entity in data.BankAccounts)
        {
            entity.Accept(yamlVisitor);
        }
        
        foreach (var entity in data.Operations)
        {
            entity.Accept(yamlVisitor);
        }
        
        foreach (var entity in data.Categories)
        {
            entity.Accept(yamlVisitor);
        }
        
        return yamlVisitor.GetResult();
    }

    protected override DataTransferDto DeserializeData(string rawData)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<DataTransferDto>(rawData);
    }
}