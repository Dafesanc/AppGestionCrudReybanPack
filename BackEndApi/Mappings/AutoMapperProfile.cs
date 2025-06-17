using AutoMapper;
using BackEndApi.DTOs;
using BackEndApi.Models;

namespace BackEndApi.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapeos para Person
            CreateMap<Person, PersonReadDto>();
            CreateMap<PersonCreateDto, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<PersonUpdateDto, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());            // Mapeos para Pet
            CreateMap<Pet, PetReadDto>();
            CreateMap<PetCreateDto, Pet>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<PetUpdateDto, Pet>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
