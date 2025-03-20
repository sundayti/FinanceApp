using Application.DTO;
using Application.TemplateMethod;
using Application.UI;
using Domain.Interfaces;

namespace Application.Facades;

public class DataTransferFacade
{
    private readonly DataTransferTemplate _transferTemplate;
    private readonly BankAccountFacade _accountFacade;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOperationRepository _operationRepository;

    public DataTransferFacade(
        DataTransferTemplate transferTemplate,
        BankAccountFacade accountFacade,
        ICategoryRepository categoryRepository,
        IOperationRepository operationRepository)
    {
        _transferTemplate = transferTemplate;
        _accountFacade = accountFacade;
        _categoryRepository = categoryRepository;
        _operationRepository = operationRepository;
    }

    public void ExportToFile(string filePath)
    {
        var data = new DataTransferDto
        {
            BankAccounts = _accountFacade.GetAllAccounts().ToList(),
            Categories = _categoryRepository.GetAll().ToList(),
            Operations = _operationRepository.GetAll().ToList()
        };

        _transferTemplate.ExportToFile(filePath, data);
    }

    public void ImportFromFile(string filePath)
    {
        var data = _transferTemplate.ImportFromFile(filePath);

        foreach (var importedAccount in data.BankAccounts)
        {
            _accountFacade.UpsertAccount(importedAccount);
        }
        foreach (var importedCategory in data.Categories)
        {
            var existing = _categoryRepository.GetById(importedCategory.Id);
            if (existing == null)
                _categoryRepository.Add(importedCategory);
            else
            {
                existing.SetName(importedCategory.Name);
                _categoryRepository.Update(existing);
            }
        }
        foreach (var importedOperation in data.Operations)
        {
            var existing = _operationRepository.GetById(importedOperation.Id);
            if (existing == null)
                _operationRepository.Add(importedOperation);
            else
                _operationRepository.Update(importedOperation);
        }

        Console.WriteLine("Данные успешно импортированы и обновлены через прокси.");
    }
}
