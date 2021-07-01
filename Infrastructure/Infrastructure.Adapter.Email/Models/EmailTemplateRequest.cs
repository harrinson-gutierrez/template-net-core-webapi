using System.Collections.Generic;

namespace Infrastructure.Adapter.Email.Models
{
    public class EmailTemplateRequest
    {
        public string Subject { get; set; }
        public string Template { get; set; }

        public List<string> Receivers { get; set; }
        public List<string> CopyReceivers { get; set; }
        public List<string> CopyReceiversHidden { get; set; } = new List<string>();

        public object Data { get; set; }

        public EmailTemplateRequest()
        {
            Receivers = new List<string>();
            CopyReceivers = new List<string>();
        }
    }
}
