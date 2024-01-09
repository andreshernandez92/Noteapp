using Backend.Data.Models;
using Backend.Data.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Data.Models.Repositories
{
    public class CategoryRepository
    {
        private readonly StoreContext _context;

        public CategoryRepository(StoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories.Find(categoryId);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void RemoveCategory(int categoryId)
        {
            var categoryToRemove = _context.Categories.Find(categoryId);
            if (categoryToRemove != null)
            {
                _context.Categories.Remove(categoryToRemove);
                _context.SaveChanges();
            }
        }
    }
}
