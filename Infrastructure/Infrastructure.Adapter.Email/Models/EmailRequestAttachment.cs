using System.Collections.Generic;

namespace Infrastructure.Adapter.Email.Models
{
    public class EmailRequestAttachment: EmailRequest
    {
        public List<EmailAttachment> Attachments { get; set; }
    }

    public class EmailAttachment
    {
        public byte[] File { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
