using Backend.Data.Models;
using Backend.Data.Models.Entities;
using Backend.Data.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
namespace Backend.Data.Models.Repositories
{
    public class CategoryRepository
    {
        private readonly StoreContext _context;
          private readonly IMapper _mapper;
        public CategoryRepository(StoreContext context, IMapper mapper)
        {
            _context = context;
           
                    _mapper = mapper;
        }

public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
{
    var categories = await _context.Categories.ToListAsync();

    return categories.Select(category => new CategoryDTO
    {
        Id = category.CategoryId,
        Name = category.Name
    }).ToList();
}

public async Task UpdateCategoryAsync(CategoryDTO updatedCategory)
{
    try
    {
        var categoryEntity = await _context.Categories
            .SingleOrDefaultAsync(c => c.CategoryId == updatedCategory.Id);
        if (categoryEntity != null)
        {
            // Log the existing and updated category information
            Console.WriteLine($"Existing Category: Id={categoryEntity.CategoryId}, Name={categoryEntity.Name}");
            Console.WriteLine($"Updated Category: Id={updatedCategory.Id}, Name={updatedCategory.Name}");

            // Update properties accordingly
            updatedCategory.Name = categoryEntity.Name;

            // Attach the entity to the context
            _context.Attach(categoryEntity);

            // Explicitly load NoteCategories
            await _context.Entry(categoryEntity)
                .Collection(c => c.NoteCategories)
                .LoadAsync();

            // Update related NoteCategories if necessary
            foreach (var noteCategory in categoryEntity.NoteCategories)
            {
                noteCategory.Category.Name = updatedCategory.Name;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        // Log or handle the exception as needed
        Console.WriteLine($"Error updating category: {ex.Message}");
        throw; // Rethrow the exception for additional debugging if needed
    }
}




    }

}
