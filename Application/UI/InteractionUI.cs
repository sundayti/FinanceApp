using Application.Commands;
using Application.Commands.Decorators;
using Application.Decorators;
using Application.Facades;
using Application.TemplateMethod;
using Domain.Entities;
using Domain.Interfaces;
using static Domain.Entities.CategoryType;

namespace Application.UI;

public class ApplicationUi
{
    private readonly BankAccountFacade _accountFacade;
    private readonly FinancialModuleFacade _financialModuleFacade;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOperationRepository _operationRepository;

    public ApplicationUi(
        BankAccountFacade accountFacade, 
        FinancialModuleFacade financialModuleFacade,
        ICategoryRepository categoryRepository,
        IOperationRepository operationRepository
    )
    {
        _accountFacade = accountFacade;
        _financialModuleFacade = financialModuleFacade;
        _categoryRepository = categoryRepository;
        _operationRepository = operationRepository;
    }

    public void Run()
    {
        while (true)
        {
            PrintMainMenu();

            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine();
            Console.Clear();
            if (choice == "0")
                break;

            switch (choice)
            {
                case "1":
                    ShowAllAccounts();
                    break;
                case "2":
                    CreateAccount();
                    break;
                case "3":
                    RenameAccount();
                    break;
                case "4":
                    DeleteAccount();
                    break;
                case "5":
                    ShowAnalytics();
                    break;
                case "6":
                    ExportImportMenu();
                    break;
                case "7":
                    CreateCategory();
                    break;
                case "8":
                    CreateOperation();
                    break;
                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }

    private void PrintMainMenu()
    {
        Console.WriteLine("\n========================================");
        Console.WriteLine("           ФИНАНСОВОЕ ПРИЛОЖЕНИЕ        ");
        Console.WriteLine("========================================");
        Console.WriteLine("1) Показать все счета");
        Console.WriteLine("2) Создать счёт");
        Console.WriteLine("3) Переименовать счёт");
        Console.WriteLine("4) Удалить счёт");
        Console.WriteLine("5) Аналитика");
        Console.WriteLine("6) Экспорт/Импорт");
        Console.WriteLine("7) Создать категорию");
        Console.WriteLine("8) Создать операцию");
        Console.WriteLine("0) Выход");
        Console.WriteLine("========================================\n");
    }

    private void ShowAllAccounts()
    {
        var accounts = _accountFacade.GetAllAccounts();

        Console.WriteLine("\nСчета:");
        foreach (var acc in accounts)
        {
            Console.WriteLine($"- {acc.Id} | {acc.Name} | баланс = {acc.Balance}");
        }
    }

    private void CreateAccount()
    {
        Console.Write("Введите название счёта: ");
        var accName = Console.ReadLine() ?? "NoName";
        
        var cmd = new CreateBankAccountCommand(_accountFacade, accName, 0);
        var decorated = new TimeMeasurementCommandDecorator(cmd);
        decorated.Execute();
        
        "Счёт создан успешно!".WriteLineWithColor(ConsoleColor.Green);
    }

    private void ShowAnalytics()
    {
        Console.Write("Введите дату начала (yyyy-MM-dd): ");
        var startStr = Console.ReadLine();
        Console.Write("Введите дату конца (yyyy-MM-dd): ");
        var endStr = Console.ReadLine();

        if (DateTime.TryParse(startStr, out DateTime start) &&
            DateTime.TryParse(endStr, out DateTime end))
        {
            start = DateTime.SpecifyKind(start, DateTimeKind.Utc);
            end = DateTime.SpecifyKind(end, DateTimeKind.Utc);

            var diff = _financialModuleFacade.GetIncomeExpenseDifference(start, end);
            Console.WriteLine($"Разница доходов и расходов: {diff}");

            var groups = _financialModuleFacade.GetOperationsGroupedByCategory(start, end);
            Console.WriteLine("Группировка операций по категориям:");
            foreach (var group in groups) 
            {
                Console.Write($"Категория: {group.CategoryName} - Общая сумма: ");
                $"{group.TotalAmount}".WriteLineWithColor(group.TotalAmount > 0
                    ? ConsoleColor.Green
                    : ConsoleColor.Red);
            }
        }
        else
        {
            "Неверный формат дат.".WriteLineWithColor(ConsoleColor.Red);
        }
    }

    private void ExportImportMenu()
    {
        Console.WriteLine("Выберите формат передачи данных: 1 - CSV, 2 - JSON, 3 - YAML");
        var formatChoice = Console.ReadLine();

        DataTransferTemplate? transferTemplate = formatChoice switch
        {
            "1" => new CsvDataTransfer(),
            "2" => new JsonDataTransfer(),
            "3" => new YamlDataTransfer(),
            _ => null
        };

        if (transferTemplate == null)
        {
            "Неверный выбор формата.".WriteLineWithColor(ConsoleColor.Red);
            return;
        }

        Console.WriteLine("Выберите действие: 1 - Экспорт, 2 - Импорт");
        var actionChoice = Console.ReadLine() ?? "";

        var dataTransferFacade = new DataTransferFacade(transferTemplate, _accountFacade, _categoryRepository, _operationRepository);

        if (actionChoice == "1")
        {
            Console.Write("Введите путь для сохранения файла: ");
            var exportPath = Console.ReadLine() ?? "";
            dataTransferFacade.ExportToFile(exportPath);
            "Экспорт выполнен успешно!".WriteLineWithColor(ConsoleColor.Green);
        }
        else if (actionChoice == "2")
        {
            Console.Write("Введите путь к файлу для импорта: ");
            var importPath = Console.ReadLine() ?? "";
            dataTransferFacade.ImportFromFile(importPath);
            "Импорт выполнен успешно!".WriteLineWithColor(ConsoleColor.Green);
        }
        else
        {
            "Неверный выбор действия.".WriteLineWithColor(ConsoleColor.Red);
        }
    }

    private void CreateCategory()
    {
        Console.WriteLine("Создание категории:");
        Console.Write("Введите название категории: ");
        var catName = Console.ReadLine() ?? "";
        Console.Write("Выберите тип категории: ");
            "1 - Доход".WriteLineWithColor(ConsoleColor.Green);
            "2 - Расход".WriteLineWithColor(ConsoleColor.Red);
        var typeInput = Console.ReadLine();
        CategoryType? catType = typeInput switch
        {
            "1" => Income,
            "2" => Expense,
            _ => null,
        };
        if (catType == null)
        {
            "Неверный выбор действия.".WriteLineWithColor(ConsoleColor.Red);
            return;
        }
        
        ICommand createCategoryCmd = new CreateCategoryCommand(_financialModuleFacade, catName, (CategoryType)catType);
        createCategoryCmd = new TimeMeasurementCommandDecorator(createCategoryCmd);
        createCategoryCmd.Execute();
        "Категория успешно создана!".WriteLineWithColor(ConsoleColor.Green);
    }

    private void CreateOperation()
    {
        Console.WriteLine("Создание операции:");

        var selectedAccId = UserInterfaceHelper.SelectAccount(_accountFacade);
        var selectedCatId = UserInterfaceHelper.SelectCategory(_financialModuleFacade);

        if (selectedAccId == Guid.Empty || selectedCatId == Guid.Empty)
        {
            "Невозможно создать операцию: выбраны неверные данные.".WriteLineWithColor(ConsoleColor.Red);
            return;
        }

        Console.Write("Введите сумму операции: ");
        var amtStr = Console.ReadLine();
        if (!decimal.TryParse(amtStr, out decimal amount))
        {
            "Неверный формат суммы.".WriteLineWithColor(ConsoleColor.Red);
            return;
        }

        Console.Write("Введите дату операции (yyyy-MM-dd): ");
        var opDateStr = Console.ReadLine();
        if (!DateTime.TryParse(opDateStr, out DateTime opDate))
        {
            "Неверный формат даты.".WriteLineWithColor(ConsoleColor.Red);
            return;
        }
        opDate = DateTime.SpecifyKind(opDate, DateTimeKind.Utc);

        Console.Write("Введите описание операции (опционально): ");
        var description = Console.ReadLine() ?? "";

        ICommand createOperationCmd = new CreateOperationCommand(
            _financialModuleFacade, 
            selectedAccId, 
            selectedCatId, 
            amount, 
            opDate, 
            description
        );
        createOperationCmd = new TimeMeasurementCommandDecorator(createOperationCmd);
        createOperationCmd.Execute();

        "Операция успешно создана!".WriteLineWithColor(ConsoleColor.Green);
    }

    private void RenameAccount()
    {
        Console.WriteLine("Выберите счёт для переименования:");
        var accountToRename = UserInterfaceHelper.SelectAccount(_accountFacade);
        if (accountToRename == Guid.Empty)
        {
            "Нет выбранного счета для переименования.".WriteLineWithColor(ConsoleColor.Red);
            return;
        }

        Console.Write("Введите новое название для счёта: ");
        var newName = Console.ReadLine() ?? "";
        ICommand renameCmd = new RenameBankAccountCommand(_accountFacade, accountToRename, newName);
        renameCmd = new TimeMeasurementCommandDecorator(renameCmd);
        renameCmd.Execute();

        "Счёт успешно переименован!".WriteLineWithColor(ConsoleColor.Green);
    }

    private void DeleteAccount()
    {
        Console.WriteLine("Выберите счёт для удаления:");
        var accountToDelete = UserInterfaceHelper.SelectAccount(_accountFacade);
        if (accountToDelete == Guid.Empty)
        {
            Console.WriteLine("Нет выбранного счета для удаления.");
            return;
        }

        ICommand deleteCmd = new DeleteBankAccountCommand(_accountFacade, accountToDelete);
        deleteCmd = new TimeMeasurementCommandDecorator(deleteCmd);
        deleteCmd.Execute();

        "Счёт успешно удалён!".WriteLineWithColor(ConsoleColor.Green);
    }
}