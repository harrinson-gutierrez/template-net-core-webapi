using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Application.Wrappers
{
    public class Response<T>
    {
        public Response() { }
        public Response(T data,string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }

        [JsonProperty(Order = 1)]
        public Guid TransactionId { get; set; } = Guid.NewGuid();
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
