using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Application.Enums
{
    [JsonConverter(typeof(CustomStringToEnumConverter))]
    public enum GrantType
    {
        [EnumMember(Value = "Password")]
        Password,
        [EnumMember(Value = "RefreshToken")]
        RefreshToken
    }

    public class CustomStringToEnumConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(reader.Value?.ToString()))
            {
                return null;
            }

            object parsedEnumValue;

            var isValidEnumValue = Enum.TryParse(objectType.GenericTypeArguments[0], reader.Value.ToString(), true, out parsedEnumValue);

            if (isValidEnumValue)
            {
                return parsedEnumValue;
            }
            else
            {
                return null;
            }
        }
    }
}
