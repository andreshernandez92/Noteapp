using AutoMapper;
using Backend.Data.DTO;
using Backend.Data.Models.Entities;
using Backend.Data.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(NoteRepository noteRepository, CategoryRepository categoryRepository, IMapper mapper)
        {
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("categories")]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            var categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return Ok(categoryDTOs);
        }

        [HttpPost("categories")]
        public ActionResult AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            _categoryRepository.AddCategory(category);
            return Ok("Category added successfully.");
        }

        [HttpDelete("categories/{id}")]
        public ActionResult RemoveCategory(int id)
        {
            _categoryRepository.RemoveCategory(id);
            return Ok("Category removed successfully.");
        }

        [HttpGet("byCategory/{categoryId}")]
        public ActionResult<IEnumerable<NoteDTO>> GetNotesByCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);

            if (category == null)
        {
            return NotFound($"Category with id {categoryId} not found.");
        }
            var notes = _noteRepository.GetNotesByCategory(category);
            var noteDTOs = _mapper.Map<IEnumerable<NoteDTO>>(notes);
            return Ok(noteDTOs);
        }
    }
}
