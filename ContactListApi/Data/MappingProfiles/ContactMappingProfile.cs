using AutoMapper;
using ContactListApi.Entities;
using ContactListApi.Models;

namespace ContactListApi.Data.MappingProfiles
{
    //https://automapper.org 
    public class ContactMappingProfile : Profile
    {
        public ContactMappingProfile()
        {
            CreateMap<Contact, ContactDto>()
                .ForMember(m => m.Category,
                c => c.MapFrom(s => s.Category.Name))
                .ForMember(m => m.SubCategory,
                c => c.MapFrom(s => s.SubCategory.Name));

            CreateMap<CreateContactDto, Contact>()
                .ForMember(s => s.Category,
                    c=> c.MapFrom(dto => new Category()
                    { Name = dto.Category}))
                .ForMember(s => s.SubCategory,
                    c => c.MapFrom(dto => new SubCategory()
                    { Name = dto.SubCategory }));
        }
    }
}
