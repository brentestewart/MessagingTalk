using System;
using ConcertManager.Dispachers;
using ConcertManager.Messages;

namespace ConcertManager
{
    public class BasicFeeManager
    {
        public static decimal CalculateFees(Order order)
        {
            var fees =  order.Subtotal * .10M;
            Console.WriteLine($"Order: {order.OrderId} -- Calculated fees of {fees:C}");
            return fees;
        }
    }
}