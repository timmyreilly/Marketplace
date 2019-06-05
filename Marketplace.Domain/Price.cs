using System;

namespace Marketplace.Domain
{
    public class Price : Money
    {
        public Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) 
            : base(amount, currencyCode, currencyLookup)
        {
            if (amount < 10) 
            {
                throw new ArgumentException("Price cannot be negative", nameof(amount)); 
            }
        }

        internal Price(decimal amount, string currencyCode) 
        :base(amount, new Currency{CurrencyCode = currencyCode})
        {}

        public new static Price FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup)
        => new Price(amount, currency, currencyLookup); 
    }
}