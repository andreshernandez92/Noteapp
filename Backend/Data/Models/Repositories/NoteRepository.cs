using Backend.Data.Models;
using Backend.Data.Models.Entities;
using Backend.Data.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
namespace Backend.Data.Models.Repositories
{
    public class NoteRepository
    {
        private readonly StoreContext _context;
   private readonly IMapper _mapper;
        public NoteRepository(StoreContext context, IMapper mapper)
        {
            _context = context;
                _mapper = mapper;
        }

        public async Task CreateNoteWithCategoriesAsync(Note noteDto, List<string> categoryNames)
{
     var noteEntity = new Note
    {
        Title = noteDto.Title,
        Content = noteDto.Content,
        Archived = noteDto.Archived,
        TimeCreated = DateTime.UtcNow,
        TimeModified = DateTime.UtcNow,
        NoteCategories = new List<NoteCategories>()
    };

    foreach (var categoryName in categoryNames)
    {
        var existingCategory = await _context.Categories.SingleOrDefaultAsync(c => c.Name == categoryName);

        if (existingCategory == null)
        {
            // If the category doesn't exist, create a new one
            existingCategory = new Category { Name = categoryName };
            _context.Categories.Add(existingCategory);
            await _context.SaveChangesAsync(); // Save changes to generate CategoryId
        }

        noteEntity.NoteCategories.Add(new NoteCategories { CategoryId = existingCategory.CategoryId });
    }

    _context.Notes.Add(noteEntity);
    await _context.SaveChangesAsync();
}
public async Task<List<NoteDTO>> GetAllNotesAsync()
{
    var notes = await _context.Notes
        .Include(n => n.NoteCategories)
        .ThenInclude(nc => nc.Category)
        .Select(n => new NoteDTO
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            Archived = n.Archived,
            TimeCreated = n.TimeCreated, // Add this line
            TimeModified = n.TimeModified, // Add this line
            Categories = n.NoteCategories
                .Select(nc => new CategoryDTO { Id = nc.Category.CategoryId, Name = nc.Category.Name })
                .ToList()
        })
        .ToListAsync();

    return notes;
}
public async Task<NoteDTO> GetNoteByIdAsync(int id)
    {
    var noteEntity = await _context.Notes
        .Include(n => n.NoteCategories)
        .ThenInclude(nc => nc.Category)
        .FirstOrDefaultAsync(n => n.Id == id);

    return noteEntity != null
        ? new NoteDTO
        {
            Id = noteEntity.Id,
            Title = noteEntity.Title,
            Content = noteEntity.Content,
            Archived = noteEntity.Archived,  // Corrected property name
            Categories = noteEntity.NoteCategories
                .Select(nc => new CategoryDTO { Id = nc.Category.CategoryId, Name = nc.Category.Name })
                .ToList()
        }
        : null;
    }
public async Task RemoveCategoryFromNoteAsync(int noteId, int categoryId)
{
    try
    {
        // Find the note with the specified ID and load NoteCategories
        var noteEntity = await _context.Notes
            .Include(n => n.NoteCategories)
            .FirstOrDefaultAsync(n => n.Id == noteId);

        if (noteEntity != null)
        {
            // Find the category within the note's categories that matches the specified category ID
            var noteCategoryToRemove = noteEntity.NoteCategories
                .FirstOrDefault(nc => nc.CategoryId == categoryId);

            if (noteCategoryToRemove != null)
            {
                // Log the existing information
                Console.WriteLine($"Removing Category: Id={noteCategoryToRemove.CategoryId}, CategoryId={noteCategoryToRemove.CategoryId}");

                // Ensure the NoteCategories list is initialized
                if (noteEntity.NoteCategories == null)
                    noteEntity.NoteCategories = new List<NoteCategories>();

                // Remove the category from the note's categories
                noteEntity.NoteCategories.Remove(noteCategoryToRemove);

                // Mark the entity state as modified
                _context.Entry(noteEntity).State = EntityState.Modified;
                _context.Entry(noteCategoryToRemove).State = EntityState.Deleted;
                // Save changes to the database
                var result= await _context.SaveChangesAsync();
            }
        }
    }
    catch (Exception ex)
    {
        // Log or handle the exception as needed
        Console.WriteLine($"Error removing category from note: {ex.Message}");
        throw; // Rethrow the exception for additional debugging if needed
    }
}

public async Task UpdateNoteWithCategoriesAsync(int id, UpdateNoteDTO updatedNote)
{
    try
    {
        var noteEntity = await _context.Notes
            .Include(n => n.NoteCategories)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (noteEntity == null)
        {
            throw new InvalidOperationException("Note not found");
        }

        // Update note details
        if (!string.IsNullOrEmpty(updatedNote.Title))
        {
            noteEntity.Title = updatedNote.Title;
        }

        if (!string.IsNullOrEmpty(updatedNote.Content))
        {
            noteEntity.Content = updatedNote.Content;
        }

        // Update Archived
        noteEntity.Archived = updatedNote.Archived;

        // Update note categories
        if (updatedNote.CategoryUpdates != null && updatedNote.CategoryUpdates.Count > 0)
        {
            if (noteEntity.NoteCategories == null)
            {
                noteEntity.NoteCategories = new List<NoteCategories>();
            }

            foreach (var categoryUpdate in updatedNote.CategoryUpdates)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == categoryUpdate.CategoryId);

                if (category == null)
                {
                    // Category not found, create a new one
                    category = new Category { Name = categoryUpdate.CategoryName };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync(); // Save changes to generate CategoryId
                }

                noteEntity.NoteCategories.Add(new NoteCategories { Category = category });
                _context.Entry(category).State = EntityState.Modified;
            }
        }

        noteEntity.TimeModified = DateTime.UtcNow;

        _context.Entry(noteEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error updating note with categories: {ex.Message}");
        throw;
    }
}

public async Task DeleteNoteAsync(int id)
{
    try
    {
        var noteEntity = await _context.Notes.FindAsync(id);

        if (noteEntity == null)
        {
            throw new InvalidOperationException("Note not found");
        }

        _context.Notes.Remove(noteEntity);

        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error deleting note: {ex.Message}");
        throw;
    }
}

public async Task<List<NoteDTO>> GetActiveNotesAsync()
{
    var activeNotes = await _context.Notes
        .Where(n => !n.Archived)
        .Select(n => new NoteDTO
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            Archived = n.Archived,
            TimeCreated = n.TimeCreated,
            TimeModified = n.TimeModified,
            Categories = n.NoteCategories
                .Select(nc => new CategoryDTO { Id = nc.Category.CategoryId, Name = nc.Category.Name })
                .ToList()
        })
        .ToListAsync();

    return activeNotes;
}

public async Task<List<NoteDTO>> GetArchivedNotesAsync()
{
    var archivedNotes = await _context.Notes
        .Where(n => n.Archived)
        .Select(n => new NoteDTO
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            Archived = n.Archived,
            TimeCreated = n.TimeCreated,
            TimeModified = n.TimeModified,
            Categories = n.NoteCategories
                .Select(nc => new CategoryDTO { Id = nc.Category.CategoryId, Name = nc.Category.Name })
                .ToList()
        })
        .ToListAsync();

    return archivedNotes;
}







    }
}
