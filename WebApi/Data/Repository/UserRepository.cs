using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data.Helper;
using WebApi.Models;


namespace WebApi.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        public UserRepository(DataContext dataContext, IOptions<AppSettings> appSettings, ILogger<UserRepository> logger)
        {
            _appSettings = appSettings.Value;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<User> Add(User user)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>(
               new OptionsWrapper<PasswordHasherOptions>(
        new PasswordHasherOptions()
        {
            CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
        })
           );
            user.Password = passwordHasher.HashPassword(user, user.Password);
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest)
        {
            var passwordHasher = new PasswordHasher<User>();
            var user = await _dataContext.Users.Where(user => user.Email == authenticateRequest.Username).FirstOrDefaultAsync();
            if(user != null)
            {
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, authenticateRequest.Password);
                var token = generateJwtToken(user);
                if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
                {
                    return new AuthenticateResponse(user, token, true);
                }
                else if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return new AuthenticateResponse(user, token, true);
                }
                else if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                {
                    return new AuthenticateResponse(null, null, false);
                }
            }

            return new AuthenticateResponse(null, null, false);
        }

        public async Task<bool> Delete(long id)
        {
            int result = 0;
            var user = await _dataContext.Users.FindAsync(id);

            if (user != null)
            {
                //Delete that post
                _dataContext.Users.Remove(user);

                //Commit the transaction
                result = await _dataContext.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }

            }
            return false;

        }

        public async Task<IEnumerable<User>> FindAll()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<User> FindByID(long id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<User> Update(User user)
        {
            user.ModifiedDate = DateTime.Now;
            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
