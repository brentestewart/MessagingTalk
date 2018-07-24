using System;
using System.Collections.Concurrent;
using System.Threading;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class ThreadedHandler<T> : IHandle<T> where T : IMessage
    {
        private IHandle<T> Handler { get; set; }
        private ConcurrentQueue<T> Queue { get; set; } = new ConcurrentQueue<T>();
        private Thread Thread { get; set; }
        public int Count => Queue.Count;

        public ThreadedHandler(IHandle<T> handler)
        {
            Handler = handler;
            Thread = new Thread(DoWork);
        }

        public void Handle(T t)
        {
            Queue.Enqueue(t);
        }

        public void Start()
        {
            Thread.Start();
        }

        public void DoWork()
        {
            while (true)
            {
                try
                {
                    if (Queue.TryDequeue(out var o))
                    {
                        Handler.Handle(o);
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ThreadedManager error: {e}");
                }
            }
        }
    }
}