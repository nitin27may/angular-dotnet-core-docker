using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AppSettings
    {
        public ConnectionString ConnectionStrings { get; set; }
        public string Secret { get; set; }
        public string SendgridAPIKey { get; set; }
    }
    public class ConnectionString
    {
        public string DefaultConnection { get; set; }

    }
}
