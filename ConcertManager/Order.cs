using System;

namespace ConcertManager
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string CreditCardNumber { get; set; }
        public string PaymentConfirmation { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Fees { get; set; }
        public decimal Taxes { get; set; }
        public int TicketCount { get; set; }
    }
}