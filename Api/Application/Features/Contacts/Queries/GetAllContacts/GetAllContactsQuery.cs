using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Contacts.Queries.GetAllContacts
{
    public class GetAllContactsQuery : IRequest<PagedResponse<IEnumerable<GetAllContactsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, PagedResponse<IEnumerable<GetAllContactsViewModel>>>
    {
        private readonly IContactRepositoryAsync _contactRepository;
        private readonly IMapper _mapper;
        public GetAllContactsQueryHandler(IContactRepositoryAsync contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllContactsViewModel>>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllContactsParameter>(request);
            var contact = await _contactRepository.GetPagedReponseAsync(validFilter.PageNumber,validFilter.PageSize);
            var contactViewModel = _mapper.Map<IEnumerable<GetAllContactsViewModel>>(contact);
            return new PagedResponse<IEnumerable<GetAllContactsViewModel>>(contactViewModel, validFilter.PageNumber, validFilter.PageSize);           
        }
    }
}
