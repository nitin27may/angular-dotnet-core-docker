using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories;

public interface IContactRepositoryAsync : IGenericRepositoryAsync<Contact>
{
    Task<bool> IsUniqueEmailAsync(string email);
}
