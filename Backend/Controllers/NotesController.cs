using AutoMapper;
using Backend.Data.DTOs;
using Backend.Data.Models.Entities;
using Backend.Data.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

          private readonly StoreContext _context;
        public NotesController(NoteRepository noteRepository, CategoryRepository categoryRepository, IMapper mapper,StoreContext context)
        {
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
             _context = context;
        }

    [HttpPost("create")]  // Explicitly specify the HTTP method
    public async Task<IActionResult> CreateNoteWithCategories([FromBody] NoteDTO noteDto)
    {
        if (noteDto == null)
        {
            return BadRequest("Invalid note data");
        }

        try
        {
            var noteEntity = _mapper.Map<Note>(noteDto);
            await _noteRepository.CreateNoteWithCategoriesAsync(noteEntity, noteDto.Categories.Select(c => c.Name).ToList());
            var createdNoteDto = _mapper.Map<NoteDTO>(noteEntity);

            return Ok(CreatedAtAction(nameof(GetNoteById), new { id = createdNoteDto.Id }, createdNoteDto));
        }
        catch (Exception ex)
        {
            // Handle exception and log if necessary
            Console.WriteLine(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById(int id)
    {
        var noteEntity = await _noteRepository.GetNoteByIdAsync(id);

        if (noteEntity == null)
        {
            return NotFound();
        }

        var noteDto = _mapper.Map<NoteDTO>(noteEntity);

        return Ok(noteDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotes()
    {
        var notes = await _noteRepository.GetAllNotesAsync();

        if (notes == null || !notes.Any())
        {
            return NotFound("No notes found.");
        }

        return Ok(notes);
    }
[HttpDelete("remove-category-from-note")]
public async Task<IActionResult> RemoveCategoryFromNoteAsync([FromQuery] int noteId, [FromQuery] int categoryId)
{
    try
    {
        await _noteRepository.RemoveCategoryFromNoteAsync(noteId, categoryId);
        return Ok();
    }
    catch (Exception ex)
    {
        // Log or handle the exception as needed
        Console.WriteLine($"Error removing category from note: {ex.Message}");
        return StatusCode(500, "Internal server error");
    }
}
[HttpPut("update-with-categories/{id}")]
public async Task<IActionResult> UpdateNoteWithCategoriesAsync(int id, [FromBody] UpdateNoteDTO updatedNote)
{
    try
    {
        await _noteRepository.UpdateNoteWithCategoriesAsync(id, updatedNote);
        return Ok();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error updating note with categories: {ex.Message}");
        return StatusCode(500, "Internal server error");
    }
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteNoteAsync(int id)
{
    try
    {
        await _noteRepository.DeleteNoteAsync(id);
        return Ok();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error deleting note: {ex.Message}");
        return StatusCode(500, "Internal server error");
    }
}

[HttpGet("archived")]
public async Task<IActionResult> GetAllArchivedNotes()
{
    try
    {
        var archivedNotes = await _noteRepository.GetArchivedNotesAsync();
        return Ok(archivedNotes);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error getting archived notes: {ex.Message}");
        return StatusCode(500, "Internal server error");
    }
}

[HttpGet("active")]
public async Task<IActionResult> GetAllActiveNotes()
{
    try
    {
        var activeNotes = await _noteRepository.GetActiveNotesAsync();
        return Ok(activeNotes);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error getting active notes: {ex.Message}");
        return StatusCode(500, "Internal server error");
    }
}

[HttpGet("by-categories")]
public async Task<IActionResult> GetNotesByCategories([FromQuery] string categoryName, [FromQuery] bool Archived)
{
    try
    {
        var notes = await _categoryRepository.GetNotesByCategoriesAsync(categoryName, Archived);
        return Ok(notes);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error getting notes by categories: {ex.Message}");
        return StatusCode(500, "Internal server error");
    }
}


    }
}
