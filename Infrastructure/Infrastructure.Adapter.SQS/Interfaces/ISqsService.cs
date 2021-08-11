using System.Threading.Tasks;

namespace Infrastructure.Adapter.SQS.Interfaces
{
    public interface ISqsService
    {
        Task PostMessageAsync<T>(string queueName, T message);
    }
}
