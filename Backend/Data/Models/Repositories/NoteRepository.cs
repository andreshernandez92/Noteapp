using Backend.Data.Models;
using Backend.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Data.Models.Repositories
{
    public class NoteRepository
    {
        private readonly StoreContext _context;

        public NoteRepository(StoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Note> GetActiveNotes()
        {
            return _context.Notes.Where(n => !n.Archived).ToList();
        }

        public IEnumerable<Note> GetArchivedNotes()
        {
            return _context.Notes.Where(n => n.Archived).ToList();
        }

        public Note GetNoteById(int noteId)
        {
            return _context.Notes.Find(noteId);
        }

                 public async Task AddNoteAsync(Note note)
        {
            // Ensure the associated categories are also added to the context
            foreach (var category in note.Categories)
            {
                // Check if the category is already in the context or not
                var existingCategory = _context.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);

                if (existingCategory != null)
                {
                    // If category already exists, attach it to prevent duplicate inserts
                    _context.Entry(existingCategory).State = EntityState.Unchanged;
                }
                else
                {
                    // If category doesn't exist, add it to the context
                    _context.Categories.Add(category);
                }
            }

            // Add the note to the context
            _context.Notes.Add(note);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public void UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
        }

        public void DeleteNote(int noteId)
        {
            var noteToDelete = _context.Notes.Find(noteId);
            if (noteToDelete != null)
            {
                _context.Notes.Remove(noteToDelete);
                _context.SaveChanges();
            }
        }
         public IEnumerable<Note> GetNotesByCategory(Category category)
        {
            return _context.Notes
                .Where(note => note.Categories.Contains(category))
                .ToList();
        }
    }
}
