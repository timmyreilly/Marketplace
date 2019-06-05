using System.Collections.Generic;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode); 
    }

    public class Currency : Value 
    {
        public string CurrencyCode {get; set;}
        public bool InUse { get; set; }
        public int DecimalPlaces { get; set; }

        public static Currency None = new Currency 
        {
            InUse = false
        };

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new System.NotImplementedException();
        }
    }
}