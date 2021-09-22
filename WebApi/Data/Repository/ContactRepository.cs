using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApi.Data.Helper;
using WebApi.Models;

namespace WebApi.Data.Repository
{
    public class ContactRepository 
    {
        private readonly DataContext _dataContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        public ContactRepository(DataContext dataContext, IOptions<AppSettings> appSettings, ILogger<ContactRepository> logger)
        {
            _appSettings = appSettings.Value;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<Contact> Add(Contact contact)
        {
            contact.CreatedDate = DateTime.Now;
            contact.ModifiedDate = DateTime.Now;
            await _dataContext.Contacts.AddAsync(contact);
            await _dataContext.SaveChangesAsync();

            return contact;
        }

        public async Task<bool> Delete(long id)
        {
            int result = 0;
            var contact = await _dataContext.Contacts.FindAsync(id);

            if (contact != null)
            {
                //Delete that post
                _dataContext.Contacts.Remove(contact);

                //Commit the transaction
                result = await _dataContext.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }

            }
            return false;
        }

        public async Task<IEnumerable<Contact>> FindAll()
        {
            return await _dataContext.Contacts.ToListAsync();
        }

        public async Task<Contact> FindByID(long id)
        {
        return await _dataContext.Contacts.FindAsync(id);
    }

        public async Task<Contact> Update(Contact contact)
        {
            contact.ModifiedDate = DateTime.Now;
            _dataContext.Contacts.Update(contact);
            await _dataContext.SaveChangesAsync();
            return contact;
        }
    }
}
