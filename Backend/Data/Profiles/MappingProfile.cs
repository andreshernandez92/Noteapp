using AutoMapper;
using Backend.Data.DTO;
using Backend.Data.Models.Entities;
namespace Backend.Data.Profiles
{
 

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<NoteDTO, Note>()
            .ForMember(dest => dest.Categories, opt => opt.Ignore()); // Ignore Categories for now, adjust as needed

        CreateMap<Note, NoteDTO>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)));

        CreateMap<Category, CategoryDTO>();

    }
}

}