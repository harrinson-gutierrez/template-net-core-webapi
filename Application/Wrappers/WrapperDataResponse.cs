using Newtonsoft.Json;

namespace Application.Wrappers
{
    public class WrapperDataResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
