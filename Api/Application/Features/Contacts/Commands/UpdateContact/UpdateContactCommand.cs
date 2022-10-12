using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Contacts.Commands.UpdateContact;

public class UpdateContactCommand : IRequest<Response<int>>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Response<int>>
    {
        private readonly IContactRepositoryAsync _contactRepository;
        public UpdateContactCommandHandler(IContactRepositoryAsync contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<Response<int>> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(command.Id);

            if (contact == null)
            {
                throw new ApiException($"Contact Not Found.");
            }
            else
            {
                contact.FirstName = command.FirstName;
                contact.LastName = command.LastName;
                contact.Mobile = command.Mobile;
                contact.City = command.City;
                contact.PostalCode = command.PostalCode;
                await _contactRepository.UpdateAsync(contact);
                return new Response<int>(contact.Id);
            }
        }
    }
}
