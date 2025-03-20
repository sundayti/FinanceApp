using Domain.Entities;
using Domain.Factories;
using Application.Services;

namespace Application.Facades
{
    public class FinancialModuleFacade
    {
        private readonly AnalyticsService _analyticsService;
        private readonly FinanceFactory _factory;
        private readonly BankAccountFacade _accountFacade;
        private readonly Domain.Interfaces.ICategoryRepository _categoryRepository;
        private readonly Domain.Interfaces.IOperationRepository _operationRepository;

        public FinancialModuleFacade(
            AnalyticsService analyticsService,
            FinanceFactory factory,
            BankAccountFacade accountFacade,
            Domain.Interfaces.ICategoryRepository categoryRepository,
            Domain.Interfaces.IOperationRepository operationRepository)
        {
            _analyticsService = analyticsService;
            _factory = factory;
            _accountFacade = accountFacade;
            _categoryRepository = categoryRepository;
            _operationRepository = operationRepository;
        }

        public decimal GetIncomeExpenseDifference(DateTime start, DateTime end)
        {
            return _analyticsService.CalculateIncomeExpenseDifference(start, end);
        }

        public IEnumerable<OperationGroup> GetOperationsGroupedByCategory(DateTime start, DateTime end)
        {
            return _analyticsService.GroupOperationsByCategory(start, end);
        }
        

        public void CreateCategory(string name, CategoryType type)
        {
            var category = _factory.CreateCategory(name, type);
            _categoryRepository.Add(category);
        }

        public void CreateOperation(Guid accountId, Guid categoryId, decimal amount, DateTime date, string description)
        {
            var category = _categoryRepository.GetById(categoryId);
            if (category == null)
                throw new Exception("Категория не найдена");

            CategoryType opType = category.Type;

            var operation = _factory.CreateOperation(opType, accountId, categoryId, amount, date, description);
            _operationRepository.Add(operation);

            if (opType == CategoryType.Income)
                _accountFacade.Deposit(accountId, amount);
            else
                _accountFacade.Withdraw(accountId, amount);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll().ToList();
        }
    }
}