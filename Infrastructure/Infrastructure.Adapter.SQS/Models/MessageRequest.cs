namespace Infrastructure.Adapter.SQS.Models
{
    public class MessageRequest
    {
        public string QueueName { get; set; }
        public string MessageBody { get; set; }
        public string MessageType { get; set; }
    }
}
