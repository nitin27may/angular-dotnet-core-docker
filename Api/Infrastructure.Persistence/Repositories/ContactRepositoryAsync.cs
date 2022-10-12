using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories;

public class ContactRepositoryAsync : GenericRepositoryAsync<Contact>, IContactRepositoryAsync
{
    private readonly DbSet<Contact> _contacts;

    public ContactRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _contacts = dbContext.Set<Contact>();
    }

    public Task<bool> IsUniqueEmailAsync(string email)
    {
        return _contacts
            .AllAsync(p => p.Email != email);
    }
}
