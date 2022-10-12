using Application.Features.Contacts.Commands.CreateContact;
using Application.Features.Contacts.Queries.GetAllContacts;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        CreateMap<Contact, GetAllContactsViewModel>().ReverseMap();
        CreateMap<CreateContactCommand, Contact>();
        CreateMap<GetAllContactsQuery, GetAllContactsParameter>();
    }
}
