using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Contacts.Commands.DeleteContactById
{
    public class DeleteContactByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteContactByIdCommandHandler : IRequestHandler<DeleteContactByIdCommand, Response<int>>
        {
            private readonly IContactRepositoryAsync _contactRepository;
            public DeleteContactByIdCommandHandler(IContactRepositoryAsync contactRepository)
            {
                _contactRepository = contactRepository;
            }
            public async Task<Response<int>> Handle(DeleteContactByIdCommand command, CancellationToken cancellationToken)
            {
                var contact = await _contactRepository.GetByIdAsync(command.Id);
                if (contact == null) throw new ApiException($"Contact Not Found.");
                await _contactRepository.DeleteAsync(contact);
                return new Response<int>(contact.Id);
            }
        }
    }
}
