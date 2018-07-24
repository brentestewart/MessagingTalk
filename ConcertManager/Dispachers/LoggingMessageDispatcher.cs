using System;
using ConcertManager.Messages;

namespace ConcertManager.Dispachers
{
    public class LoggingMessageDispatcher : IPublisher
    {
        public IPublisher InnerPublisher { get; }

        public LoggingMessageDispatcher(IPublisher innerPublisher)
        {
            InnerPublisher = innerPublisher;
        }

        public void Publish<T>(T t) where T : IMessage
        {
            Console.WriteLine($"Publishing: {t} \t\t\t{t.Order.OrderId}");
            InnerPublisher.Publish(t);    
        }

        public void Subscribe<T>(IHandle<T> handler) where T : IMessage
        {
            InnerPublisher.Subscribe(handler);
        }
    }
}