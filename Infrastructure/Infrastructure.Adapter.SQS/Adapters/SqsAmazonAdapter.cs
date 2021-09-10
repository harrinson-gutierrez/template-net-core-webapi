using Amazon.SQS;
using Amazon.SQS.Model;
using Infrastructure.Adapter.Email.Settings;
using Infrastructure.Adapter.SQS.Interfaces;
using Infrastructure.Adapter.SQS.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Adapter.SQS.Adapters
{
    public class SqsAmazonAdapter : ISqsService
	{
		private readonly IAmazonSQS Client;
		private readonly IOptions<SqsOptions> SqsConfiguration;
		private readonly ILogger<SqsAmazonAdapter> Logger;
		private readonly ConcurrentDictionary<string, string> QueueUrlCache;

		public SqsAmazonAdapter(IOptions<SqsOptions> sqsConfiguration,
								  IAmazonSQS client,
								  ILogger<SqsAmazonAdapter> logger)
		{
			Client = client;
			SqsConfiguration = sqsConfiguration;
			Logger = logger;
			QueueUrlCache = new ConcurrentDictionary<string, string>();
		}

		public async Task PostMessageAsync<T>(string queueName, T message)
		{
			var queueUrl = await GetQueueUrl(queueName);

			try
			{
				var sendMessageRequest = new SendMessageRequest
				{
					QueueUrl = queueUrl,
					MessageBody = JsonConvert.SerializeObject(message),
					MessageAttributes = SqsMessageTypeAttribute.CreateAttributes<T>()
				};

				await Client.SendMessageAsync(sendMessageRequest);
				Logger.LogInformation("Message is sended");
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, $"Failed to PostMessagesAsync to queue '{queueName}'. Exception: {ex.Message}");
				throw;
			}
		}

		private async Task<string> GetQueueUrl(string queueName)
		{
			if (string.IsNullOrEmpty(queueName))
			{
				throw new ArgumentException("Queue name should not be blank.");
			}

			if (QueueUrlCache.TryGetValue(queueName, out var result))
			{
				return result;
			}

			try
			{
				var response = await Client.GetQueueUrlAsync(queueName);
				return QueueUrlCache.AddOrUpdate(queueName, response.QueueUrl, (q, url) => url);
			}
			catch (QueueDoesNotExistException ex)
			{
				throw new InvalidOperationException($"Could not retrieve the URL for the queue '{queueName}' as it does not exist or you do not have access to it.", ex);
			}
		}
	}
}
