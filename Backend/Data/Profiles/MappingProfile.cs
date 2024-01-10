using AutoMapper;
using Backend.Data.DTO;
using Backend.Data.Models.Entities;

namespace Backend.Data.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping for Note and NoteDTO
            CreateMap<Note, NoteDTO>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

            CreateMap<NoteDTO, Note>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

            // Mapping for Category and CategoryDTO
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
        }
    }
}


