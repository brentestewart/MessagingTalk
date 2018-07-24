using System;
using System.Collections.Generic;
using System.Text;
using NServiceBus;

namespace ConcertManager.Messages
{
    public interface IMessage
    {
        Guid MessageId { get; }
        Order Order { get; set; }
    }

    public abstract class Message : IMessage
    {
        public Guid MessageId { get; } = new Guid();
        public Order Order { get; set; }
    }

    public class Command : Message { }
    public class Event : Message { }


    public class ReserveTickets : Command { }
    public class CalculateFees : Command { }
    public class CalculateTaxes : Command { }
    public class ChargeCreditCard : Command, ICommand { }
    public class CommitOrder : Command { }

    public class OrderPlaced : Event { }
    public class TicketsReserved : Event { }
    public class FeesCalculated : Event { }
    public class TaxesCalculated : Event { }
    public class CreditCardCharged : Event, IEvent { }
    public class OrderCommited : Event { }
}
