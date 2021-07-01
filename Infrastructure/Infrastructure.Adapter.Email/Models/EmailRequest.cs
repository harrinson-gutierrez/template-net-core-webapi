using System.Collections.Generic;

namespace Infrastructure.Adapter.Email.Models
{
    public class EmailRequest
    {
        
        public string Subject { get; set; }
        
        public string HtmlBody { get; set; }
        
        public string TextBody { get; set; }
        
        public List<string> Receivers { get; set; }

        public List<string> CopyReceivers { get; set; }

        public List<string> CopyReceiversHidden { get; set; } = new List<string>();

        public EmailRequest()
        {
            Receivers = new List<string>();
            CopyReceivers = new List<string>();
        }
    }
}
