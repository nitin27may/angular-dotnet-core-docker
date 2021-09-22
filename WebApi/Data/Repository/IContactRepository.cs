using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data.Repository
{
    public interface IContactRepository
    {
        Task<Contact> Add(Contact contact);
        Task<bool> Delete(long id);
        Task<Contact> Update(Contact item);
        Task<Contact> FindByID(long id);
        Task<IEnumerable<Contact>> FindAll();
    }
}
