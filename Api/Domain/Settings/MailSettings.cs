using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Settings
{
    public class SendGridSettings
    {
        public string EmailFrom { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
    }
}
