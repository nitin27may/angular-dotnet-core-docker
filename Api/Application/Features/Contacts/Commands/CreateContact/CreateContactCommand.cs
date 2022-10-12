using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Contacts.Commands.CreateContact;

public partial class CreateContactCommand : IRequest<Response<int>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
}
public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Response<int>>
{
    private readonly IContactRepositoryAsync _contactRepository;
    private readonly IMapper _mapper;
    public CreateContactCommandHandler(IContactRepositoryAsync contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = _mapper.Map<Contact>(request);
        await _contactRepository.AddAsync(contact);
        return new Response<int>(contact.Id);
    }
}
