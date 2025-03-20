using Application.Facades;
using Domain.Entities;

namespace Application.UI;

public static class UserInterfaceHelper
{
    public static Guid SelectAccount(BankAccountFacade accountFacade)
    {
        var accounts = accountFacade.GetAllAccounts().ToList();
        if (!accounts.Any())
        {
            Console.WriteLine("Нет доступных счетов.");
            return Guid.Empty;
        }

        Console.WriteLine("Доступные счета:");
        for (int i = 0; i < accounts.Count; i++)
        {
            var acc = accounts[i];
            Console.WriteLine($"{i + 1}) {acc.Name} (ID: {acc.Id}, баланс: {acc.Balance})");
        }
        Console.Write("Выберите номер счета: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= accounts.Count)
        {
            return accounts[index - 1].Id;
        }
        else
        {
            "Неверный выбор.".WriteLineWithColor(ConsoleColor.Red);
            return Guid.Empty;
        }
    }
    
    public static Guid SelectCategory(FinancialModuleFacade financialFacade)
    {
        var categories = financialFacade.GetAllCategories().ToList();
        if (!categories.Any())
        {
            Console.WriteLine("Нет доступных категорий.");
            return Guid.Empty;
        }

        Console.WriteLine("Доступные категории:");
        for (int i = 0; i < categories.Count; i++)
        {
            var cat = categories[i];
            Console.Write($"{i + 1}) ");
            $"{cat.Name}".WriteWithColor(cat.Type == CategoryType.Income ? ConsoleColor.Green : ConsoleColor.Red);
            Console.WriteLine($" (ID: {cat.Id})");
        }
        Console.Write("Выберите номер категории: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= categories.Count)
        {
            return categories[index - 1].Id;
        }
        else
        {
            "Неверный выбор.".WriteLineWithColor(ConsoleColor.Red);
            return Guid.Empty;
        }
    }
}