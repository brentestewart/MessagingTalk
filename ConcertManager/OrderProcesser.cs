using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class OrderProcessor : IOrderProcessor
    {
        public static void ProcessOrder(Order order)
        {
            TicketManager.ReserveTickets(order);
            order.Fees = BasicFeeManager.CalculateFees(order);
            order.Taxes = TaxManager.CalculateTaxes(order);
            order.PaymentConfirmation = PaymentManager.ChargeCrditCard(order);
        }
    }
}