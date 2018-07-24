using System;
using System.Collections.Generic;
using System.Text;
using ConcertManager.Messages;

namespace ConcertManager.Dispachers
{
    public interface IPublisher
    {
        void Publish<T>(T t) where T : IMessage;
        void Subscribe<T>(IHandle<T> handler) where T : IMessage;
    }

        public interface IHandle<T> where T : IMessage
    {
        void Handle(T t);
    }

    public class LoggingMessageDispatcher : IPublisher
    {
        public IPublisher InnerPublisher { get; }

        public LoggingMessageDispatcher(IPublisher innerPublisher)
        {
            InnerPublisher = innerPublisher;
        }

        public void Publish<T>(T t) where T : IMessage
        {
            Console.WriteLine($"Publishing: {t}");
            InnerPublisher.Publish(t);    
        }

        public void Subscribe<T>(IHandle<T> handler) where T : IMessage
        {
            InnerPublisher.Subscribe(handler);
        }
    }

        public class MessageDispatcher : IPublisher
    {
        private Dictionary<string, List<Action<IMessage>>> Handlers { get; set; } = new Dictionary<string, List<Action<IMessage>>>();

        public void Publish<T>(T t) where T : IMessage
        {
            var typeName = t.GetType()?.FullName ?? "";

            if (Handlers.TryGetValue(typeName, out var allHandlers))
            {
                foreach (var currentHandler in allHandlers)
                {
                    currentHandler(t);
                }
            }
        }

        public void Subscribe<T>(IHandle<T> handler) where T : IMessage
        {
            var handlers = FindOrCreateHandlers(typeof(T).FullName);
            handlers.Add(m => handler.Handle((T)m));
        }

        private List<Action<IMessage>> FindOrCreateHandlers(string topic)
        {
            if (!Handlers.TryGetValue(topic, out var handlers))
            {
                handlers = new List<Action<IMessage>>();
                Handlers[topic] = handlers;
            }

            return handlers;
        }
    }
}
