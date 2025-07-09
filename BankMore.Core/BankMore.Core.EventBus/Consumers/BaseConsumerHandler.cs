using BankMore.Core.EventBus.Models;
using System.Text.Json;

namespace BankMore.Core.EventBus.Consumers;

public class BaseConsumerHandler
{
    protected static TRequest? GetDeserializeBodyMessage<TRequest>(Body body, bool propertyNameCaseInsensitive = true)
    {
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = propertyNameCaseInsensitive };

        return body.Payload is not JsonElement json ? default : json.Deserialize<TRequest>(jsonOptions);
    }
}
