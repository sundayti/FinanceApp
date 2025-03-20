using System.Text.Encodings.Web;
using System.Text.Json;
using Application.DTO;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Visitors
{
    public class JsonExportVisitor : IVisitor
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
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            return JsonSerializer.Serialize(_dto, options);
        }
    }
}