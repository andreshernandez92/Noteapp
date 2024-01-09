using Backend.Data.Models;
using Backend.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Data.Repositories
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

        public void AddNote(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
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
    }
}
