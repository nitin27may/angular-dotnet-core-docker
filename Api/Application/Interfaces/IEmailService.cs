using Application.DTOs.Email;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}
