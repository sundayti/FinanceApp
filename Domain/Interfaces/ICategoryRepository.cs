using Domain.Entities;

namespace Domain.Interfaces;

public interface ICategoryRepository
{
    void Add(Category category);
    Category? GetById(Guid id);
    IEnumerable<Category> GetAll();
    void Update(Category category);
    void Delete(Guid id);
}