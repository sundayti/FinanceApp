using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Proxy;

public class CategoryRepositoryProxy : ICategoryRepository
{
    private readonly ICategoryRepository _realRepository;
    private readonly Dictionary<Guid, Category> _cache = new();

    public CategoryRepositoryProxy(ICategoryRepository realRepository)
    {
        _realRepository = realRepository;
        LoadCache();
    }

    public void Add(Category category)
    {
        _realRepository.Add(category);
        _cache[category.Id] = category;
    }

    public Category? GetById(Guid id)
    {
        if (_cache.TryGetValue(id, out var cached))
            return cached;

        var fromDb = _realRepository.GetById(id);
        if (fromDb != null)
            _cache[fromDb.Id] = fromDb;
        return fromDb;
    }

    public IEnumerable<Category> GetAll()
    {
        return _cache.Values;
    }

    public void Update(Category category)
    {
        _realRepository.Update(category);
        _cache[category.Id] = category;
    }

    public void Delete(Guid id)
    {
        _realRepository.Delete(id);
        _cache.Remove(id);
    }

    private void LoadCache()
    {
        var allCategories = _realRepository.GetAll();
        foreach (var cat in allCategories)
        {
            _cache[cat.Id] = cat;
        }
    }
}
