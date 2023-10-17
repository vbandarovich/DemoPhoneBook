using AutoMapper;
using PhoneBookApp.Common.Models;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContactModel, PersonContactModel>()
                .ReverseMap();

            CreateMap<ContactModel, PublicOrganisationContactModel>()
                .ReverseMap();

            CreateMap<ContactModel, PrivateOrganisationContactModel>()
                .ReverseMap();

            CreateMap<PersonContactModel, PhoneBookEntity>()
                .ReverseMap();

            CreateMap<PublicOrganisationContactModel, PhoneBookEntity>()
                .ReverseMap();

            CreateMap<PrivateOrganisationContactModel, PhoneBookEntity>()
                .ReverseMap();

            CreateMap<ContactModel, PhoneBookEntity>()
                .ReverseMap();
        }
    }
}
