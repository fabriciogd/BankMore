using BankMore.Core.EventBus.Models;
using Rebus.Messages;
using Rebus.Serialization;
using System.Text;
using System.Text.Json;

namespace BankMore.Core.EventBus.Helpers;

public sealed class EventBusSerializer(ISerializer _serializer) : ISerializer
{
    public async Task<TransportMessage> Serialize(Message message)
    {
        return await _serializer.Serialize(message).ConfigureAwait(false);
    }

    public Task<Message> Deserialize(TransportMessage? transportMessage)
    {
        var json = Encoding.UTF8.GetString(transportMessage.Body);
        var body = JsonSerializer.Deserialize<Body>(json);
        return Task.FromResult(new Message(transportMessage.Headers, body));
    }
}
