using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Contacts.Queries.GetAllContacts
{
    public class GetAllContactsViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}
