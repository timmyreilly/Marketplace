using System.Collections.Generic;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public interface ICurrencyLookup
    {
        CurrencyDetails FindCurrency(string currencyCode); 
    }
    
    public class CurrencyDetails : Value 
    {
        public string CurrencyCode {get; set;}
        public bool InUse { get; set; }
        public int DecimalPlaces { get; set; }

        public static CurrencyDetails None = new CurrencyDetails 
        {
            InUse = false
        };

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new System.NotImplementedException();
        }
    }
}