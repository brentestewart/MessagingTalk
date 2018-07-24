using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    internal interface IOrderProcessor :
        IHandle<OrderPlaced>,
        IHandle<TicketsReserved>,
        IHandle<FeesCalculated>,
        IHandle<TaxesCalculated>,
        IHandle<CreditCardCharged>
    {
    }
}