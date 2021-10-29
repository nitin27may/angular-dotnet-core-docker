using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Contacts.Commands.CreateContact
{
    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        private readonly IContactRepositoryAsync contactRepository;

        public CreateContactCommandValidator(IContactRepositoryAsync contactRepository)
        {
            this.contactRepository = contactRepository;

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
       

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(120).WithMessage("{PropertyName} must not exceed 120 characters.")
                .MustAsync(IsUniqueEmail).WithMessage("{PropertyName} already exists.");
            RuleFor(p => p.Mobile)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .NotNull()
               .MaximumLength(13).WithMessage("{PropertyName} must not exceed 13 characters.");

        }

        private async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await contactRepository.IsUniqueEmailAsync(email);
        }
    }
}
