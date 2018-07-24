using System.Collections.Generic;
using System.Linq;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class RoundRobinDispatcher<T> : IHandle<T> where T : Message
    {
        private List<IHandle<T>> Handlers { get; set; }
        private int NextHandler { get; set; }

        public RoundRobinDispatcher(IEnumerable<IHandle<T>> handlers)
        {
            Handlers = handlers.ToList();
        }

        public void Handle(T t)
        {
            Handlers[NextHandler].Handle(t);
            NextHandler = (NextHandler + 1) % Handlers.Count;
        }
    }
}