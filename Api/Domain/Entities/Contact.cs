using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Contact : BaseEntity
    {
        //[Required]
        public string FirstName { get; set; }
        //[Required]
        public string LastName { get; set; }

        //[Required]
        public string Email { get; set; }

       // [Required]
        public string Mobile { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}
