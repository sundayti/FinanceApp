using System;
using Application.Facades;
using Application.Infrastructure;
using Application.Repositories;
using Application.Proxy;
using Application.Services;
using Application.UI;
using Domain.Factories;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp
{
    internal static class Program
    {
        private static void Main()
        {
            var services = new ServiceCollection();

            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Main")
                                   ?? "Host=localhost;Port=5432;Database=financedb;Username=postgres;Password=postgres";
            services.AddDbContext<FinanceDbContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<BankAccountRepository>();
            services.AddScoped<CategoryRepository>();
            services.AddScoped<OperationRepository>();
            
            services.AddScoped<IBankAccountRepository>(sp =>
                new BankAccountRepositoryProxy(sp.GetRequiredService<BankAccountRepository>()));
            services.AddScoped<ICategoryRepository>(sp =>
                new CategoryRepositoryProxy(sp.GetRequiredService<CategoryRepository>()));
            services.AddScoped<IOperationRepository>(sp =>
                new OperationRepositoryProxy(sp.GetRequiredService<OperationRepository>()));

            services.AddScoped<FinanceFactory>();
            services.AddScoped<BankAccountFacade>();
            services.AddScoped<AnalyticsService>();
            services.AddScoped<FinancialModuleFacade>();
            services.AddScoped<ApplicationUi>();

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
                dbContext.Database.Migrate();

                var appUi = scope.ServiceProvider.GetRequiredService<ApplicationUi>();
                appUi.Run();
            }

            Console.WriteLine("Завершение работы приложения.");
            Console.ReadKey();
        }
    }
}