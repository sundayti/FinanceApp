using Domain.Entities;

namespace Application.DTO;

public class DataTransferDto
{
    public List<BankAccount> BankAccounts { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Operation> Operations { get; set; } = new();
}