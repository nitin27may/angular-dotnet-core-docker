using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Contacts.Queries.GetContactById
{
    public class GetContactByIdQuery : IRequest<Response<Contact>>
    {
        public int Id { get; set; }

        public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, Response<Contact>>
        {
            private readonly IContactRepositoryAsync _contactRepository;

            public GetContactByIdQueryHandler(IContactRepositoryAsync contactRepository)
            {
                _contactRepository = contactRepository;
            }

            public async Task<Response<Contact>> Handle(GetContactByIdQuery query, CancellationToken cancellationToken)
            {
                var contact = await _contactRepository.GetByIdAsync(query.Id);
                if (contact == null) throw new ApiException($"Contact Not Found.");
                return new Response<Contact>(contact);
            }
        }
    }
}