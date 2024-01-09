using AutoMapper;
using Backend.Data.DTO;
using Backend.Data.Models.Entities;
using Backend.Data.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public NotesController(NoteRepository noteRepository, CategoryRepository categoryRepository, IMapper mapper)
        {
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("active")]
        public ActionResult<IEnumerable<NoteDTO>> GetActiveNotes()
        {
            var activeNotes = _noteRepository.GetActiveNotes();
            var activeNotesDTO = _mapper.Map<IEnumerable<NoteDTO>>(activeNotes);
            return Ok(activeNotesDTO);
        }

        [HttpGet("archived")]
        public ActionResult<IEnumerable<NoteDTO>> GetArchivedNotes()
        {
            var archivedNotes = _noteRepository.GetArchivedNotes();
            var archivedNotesDTO = _mapper.Map<IEnumerable<NoteDTO>>(archivedNotes);
            return Ok(archivedNotesDTO);
        }

        [HttpPost]
        public ActionResult AddNote([FromBody] NoteDTO noteDTO)
        {
            var note = _mapper.Map<Note>(noteDTO);
            _noteRepository.AddNote(note);
            return Ok("Note added successfully.");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateNote(int id, [FromBody] NoteDTO updatedNoteDTO)
        {
            var existingNote = _noteRepository.GetNoteById(id);

            if (existingNote == null)
                return NotFound();

            _mapper.Map(updatedNoteDTO, existingNote);

            _noteRepository.UpdateNote(existingNote);

            return Ok("Note updated successfully.");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteNote(int id)
        {
            _noteRepository.DeleteNote(id);
            return Ok("Note deleted successfully.");
        }

    }
}
