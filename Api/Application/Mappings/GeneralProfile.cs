using Application.Features.Contacts.Commands.CreateContact;
using Application.Features.Contacts.Queries.GetAllContacts;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Contact, GetAllContactsViewModel>().ReverseMap();
            CreateMap<CreateContactCommand, Contact>();
            CreateMap<GetAllContactsQuery, GetAllContactsParameter>();
        }
    }
}
