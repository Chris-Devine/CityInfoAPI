using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.api.Services
{
    public class CloudMailService: IMailService
    {
        private string _mailTo = "chris.devine@pimss.com";
        public string _mailFrom = "api@pimss.com";

        public void Send(string subject, string message)
        {
            // Send Mail - Mimic action by outputing to window
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with CloudMailService");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
