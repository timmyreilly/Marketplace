using System;

namespace Marketplace.Domain
{
    public class Price : Money
    {
        public Price(decimal amount) : base(amount)
        {
            if (amount < 10) 
            {
                throw new ArgumentException("Price cannot be negative", nameof(amount)); 
            }
        }
    }
}