using AutoMapper;
using Backend.Data.DTO;
using Backend.Data.Models.Entities;
namespace Backend.Data.Profiles
{
 

public class MappingProfile : Profile
{
    public MappingProfile()
    {
       CreateMap<Note, NoteDTO>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)));

        CreateMap<Category, CategoryDTO>();

    }
}

}