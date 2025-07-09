using BankMore.Core.EventBus.Models;
using Rebus.Handlers;

namespace BankMore.Core.EventBus.Contracts;

public interface IConsumer : IHandleMessages<Body>
{
    public string Name { get; }
}