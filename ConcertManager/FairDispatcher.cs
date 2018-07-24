using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class FairDispatcher<T> : IHandle<T> where T : Message
    {
        private List<ThreadedHandler<T>> Handlers { get; set; }

        public FairDispatcher(IEnumerable<ThreadedHandler<T>> handlers)
        {
            Handlers = handlers.ToList();
        }

        public void Handle(T t)
        {
            while (true)
            {
                foreach (var handler in Handlers)
                    if (handler.Count < 5)
                    {
                        handler.Handle(t);
                        return;
                    }

                Thread.Sleep(1);
            }
        }
    }
}