using System;
using System.Collections.Generic;
using ConcertManager.Dispachers;
using ConcertManager.Messages;
using LiteDB;

namespace ConcertManager.Repositories
{
    public class OrderEventStore :
        IHandle<OrderPlaced>,
        IHandle<TicketsReserved>, 
        IHandle<FeesCalculated>,
        IHandle<TaxesCalculated>,
        IHandle<CreditCardCharged>,
        IHandle<OrderCommited>
    {
        private int TicketsSold { get; set; }
        public LiteDatabase Database { get; set; }
        private List<OrderEvent> Events { get; set; }
        public OrderEventStore()
        {
            Database = new LiteDatabase("Orders.db");
            LoadEvents();
        }

        private void LoadEvents()
        {
            var orderEventCollection = Database.GetCollection<OrderEvent>("orderevents");
            var allEvents = orderEventCollection.Find(Query.All());
            Events = new List<OrderEvent>(allEvents);
        }

        public int GetSoldTicketCount()
        {
            return TicketsSold;
        }

        private void StoreEvent(OrderEvent evnt)
        {
            var events = Database.GetCollection<OrderEvent>("orderevents");
            events.Insert(evnt);
        }

        public void Handle(TicketsReserved t)
        {
            Console.WriteLine("Creating Event source 'Tickets Reserved'");
            var evnt = new TicketsReservedEvent {OrderId = t.Order.OrderId, TicketCount = t.Order.TicketCount};
            StoreEvent(evnt);
        }

        public void Handle(FeesCalculated t)
        {
            Console.WriteLine("Creating Event source 'Fees Calculated'");
            var evnt = new FeesCalculatedEvent {OrderId = t.Order.OrderId, Fees = t.Order.Fees};
            StoreEvent(evnt);
        }

        public void Handle(OrderPlaced t)
        {
            Console.WriteLine("Creating Event source 'Order Placed'");
            var evnt = new OrderStartedEvent() { OrderId = t.Order.OrderId };
            StoreEvent(evnt);
        }

        public void Handle(TaxesCalculated t)
        {
            Console.WriteLine("Creating Event source 'Taxed Calculated'");
            var evnt = new TaxesCalculatedEvent { OrderId = t.Order.OrderId, Taxes = t.Order.Taxes };
            StoreEvent(evnt);
        }

        public void Handle(CreditCardCharged t)
        {
            Console.WriteLine("Creating Event source 'Credit Card Charged'");
            var evnt = new CreditCardChargedEvent() { OrderId = t.Order.OrderId, ConfirmationNumber = t.Order.PaymentConfirmation };
            StoreEvent(evnt);
        }

        public void Handle(OrderCommited t)
        {
            Console.WriteLine("Creating Event source 'OrderCommited'");
            var evnt = new OrderCompleteEvent() { OrderId = t.Order.OrderId };
            StoreEvent(evnt);
        }
    }

    public class OrderEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public Guid OrderId { get; set; }
    }

    public class OrderStartedEvent : OrderEvent { }

    public class OrderCreatedEvent : OrderEvent
    {
        public string CreditCardNumber { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class TicketsReservedEvent : OrderEvent
    {
        public int TicketCount { get; set; }
    }

    public class FeesCalculatedEvent : OrderEvent
    {
        public decimal Fees { get; set; }
    }

    public class TaxesCalculatedEvent : OrderEvent
    {
        public decimal Taxes { get; set; }
    }

    public class CreditCardChargedEvent : OrderEvent
    {
        public string ConfirmationNumber { get; set; }
    }

    public class OrderCompleteEvent : OrderEvent { }

}