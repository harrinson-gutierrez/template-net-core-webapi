using Infrastructure.Adapter.SQS.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Infrastructure.Adapter.SQS.Test
{
    public class Tests
    {
        private readonly ISqsService SqsService;
        private IServiceCollection Services;

        public Tests(ISqsService sqsService)
        {
            SqsService = sqsService;
        }

        [SetUp]
        public void Setup()
        {
            Services = new ServiceCollection();

            Services.AddSQSAdapter(ConfigurationStarted.InitConfiguration());

            Services.BuildServiceProvider();
        }

        [Test]
        public async Task Test1()
        {
            await SqsService.PostMessageAsync("prueba", new TestMessage()
            {
                Test = "prueba"
            });
            Assert.Pass();
        }
    }

    public class TestMessage
    {
        public string Test { get; set; }
    }
}