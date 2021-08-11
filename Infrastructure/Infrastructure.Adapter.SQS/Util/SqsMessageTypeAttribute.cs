using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Adapter.SQS.Util
{
	public static class SqsMessageTypeAttribute
	{
		private const string AttributeName = "MessageType";

		public static string GetMessageTypeAttributeValue(this Dictionary<string, MessageAttributeValue> attributes)
		{
			return attributes.SingleOrDefault(x => x.Key == AttributeName).Value?.StringValue;
		}

		public static Dictionary<string, MessageAttributeValue> CreateAttributes<T>()
		{
			return CreateAttributes(typeof(T).Name);
		}

		public static Dictionary<string, MessageAttributeValue> CreateAttributes(string messageType)
		{
			return new Dictionary<string, MessageAttributeValue>
		{
			{
				AttributeName, new MessageAttributeValue
				{
					DataType = nameof(String),
					StringValue = messageType
				}
			}
		};
		}
	}
}
