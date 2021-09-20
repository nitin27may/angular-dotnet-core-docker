using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data.Repository
{
    public interface IUserRepository
    {
        Task<User> Add(User item);
        Task<bool> Delete(long id);
        Task<User> Update(User item);
        Task<User> FindByID(long id);
        Task<IEnumerable<User>> FindAll();

        Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest);
    }
}
