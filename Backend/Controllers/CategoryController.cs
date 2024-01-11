using AutoMapper;
using Backend.Data.DTOs;
using Backend.Data.Models.Entities;
using Backend.Data.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(NoteRepository noteRepository, CategoryRepository categoryRepository, IMapper mapper)
        {
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        return Ok(categories);
    }

    }
}
