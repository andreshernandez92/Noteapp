using AutoMapper;
using Backend.Data.DTO;
using Backend.Data.Models.Entities;

namespace Backend.Data.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
        
        // Note to NoteDTO
        CreateMap<Note, NoteDTO>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.NoteCategories.Select(nc => nc.Category)));

        // Category to CategoryDTO
        CreateMap<Category, CategoryDTO>();

        // NoteDTO to Note
        CreateMap<NoteDTO, Note>()
            .ForMember(dest => dest.NoteCategories, opt => opt.MapFrom(src => src.Categories.Select(catDto => new NoteCategories { Category = new Category { Name = catDto.Name } })));

        // CategoryDTO to Category
        CreateMap<CategoryDTO, Category>()
            .ForMember(dest => dest.NoteCategories, opt => opt.Ignore()); // Ignore NoteCategories during mapping

    }
}
}


