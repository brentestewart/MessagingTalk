using System;
using System.Collections.Generic;
using System.Text;

namespace ConcertManager.Messages
{
    public interface IMessage
    {
        Guid MessageId { get; }
    }

    public abstract class Message : IMessage
    {
        public Guid MessageId { get; } = new Guid();
    }

    public class Command : Message
    {
    }

    public class Event : Message { }

    public class OrderPlaced : Event
    {
        public Order Order { get; set; }
    }

    public class ReserveTickets : Command
    {
        public Order Order { get; set; }
    }

    public class TicketsReserved : Event
    {
        public Order Order { get; set; }
    }

    public class CalculateFees : Command
    {
        public Order Order { get; set; }
    }

    public class FeesCalculated : Event
    {
        public Order Order { get; set; }
    }

    public class CalculateTaxes : Command
    {
        public Order Order { get; set; }
    }

    public class TaxesCalculated : Event
    {
        public Order Order { get; set; }
    }

    public class ChargeCreditCard : Command
    {
        public Order Order { get; set; }
    }

    public class CreditCardCharged : Event
    {
        public Order Order { get; set; }
    }

    public class CommitOrder : Command
    {
        public Order Order { get; set; }
    }

    public class OrderCommited : Event
    {
        public Order Order { get; set; }
    }
}
