using Application.Facades;
using Application.Infrastructure;
using Application.Repositories;
using Application.Proxy;
using Application.Repositories;
using Application.Services;
using Application.UI;
using Domain.Factories;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp
{
    internal static class Program
    {
        private static void Main()
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Main")
                                   ?? "Host=localhost;Port=5432;Database=financedb;Username=postgres;Password=postgres";
            var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            using var dbContext = new FinanceDbContext(optionsBuilder.Options);
            dbContext.Database.Migrate();

            IBankAccountRepository realAccountRepo = new BankAccountRepository(dbContext);
            ICategoryRepository realCategoryRepo = new CategoryRepository(dbContext);
            IOperationRepository realOperationRepo = new OperationRepository(dbContext);
            
            IBankAccountRepository proxyAccountRepo = new BankAccountRepositoryProxy(realAccountRepo);
            ICategoryRepository proxyCategoryRepo = new CategoryRepositoryProxy(realCategoryRepo);
            IOperationRepository proxyOperationRepo = new OperationRepositoryProxy(realOperationRepo);
            
            var factory = new FinanceFactory();
            var accountFacade = new BankAccountFacade(proxyAccountRepo, factory);

            var analyticsService = new AnalyticsService(dbContext);

            var financialModuleFacade = new FinancialModuleFacade(
                analyticsService, 
                factory,
                accountFacade,
                proxyCategoryRepo,
                proxyOperationRepo
            );

            var appUi = new ApplicationUi(accountFacade, financialModuleFacade, proxyCategoryRepo, proxyOperationRepo);
            appUi.Run();

            Console.WriteLine("Завершение работы приложения.");
            Console.ReadKey();
        }
    }
}