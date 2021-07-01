using System.IO;

namespace Infrastructure.Adapter.Email.Models
{
    public class EmailRequestAttachment: EmailRequest
    {
        public MemoryStream File { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
