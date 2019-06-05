using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Money : Value
    {
        private const string DefaultCurrency = "EUR";
        public string CurrencyCode { get; }

        // public Money(decimal amount)
        // {
        //     Amount = amount;
        // }
        // WE MUST PROTECT THE CONSTRUCTOR. MAKE THE IMPLICIT EXPLICIT! 


        // These are factory methods: 
        // public static Money FromDecimal(decimal amount, string currency = DefaultCurrency) => new Money(amount, currency);
        // public static Money FromString(string amount, string currency = DefaultCurrency) => new Money(decimal.Parse(amount), currency);

        // These are our factory methods using our currency service: 
        public static Money FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup) =>
            new Money(amount, currency, currencyLookup);

        public static Money FromString(string amount, string currency, ICurrencyLookup currencyLookup) => 
            new Money(decimal.Parse(amount), currency, currencyLookup); 
        // protected Money(decimal amount, string currencyCode = "EUR")  
        // {
        //     if (decimal.Round(amount, 2) != amount) 
        //     {
        //         throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot have more than two decimals"); 
        //     }

        //     Amount = amount; 
        //     CurrencyCode = currencyCode; 
        // }

        // new constructor with currency lookup service 
        protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentNullException(nameof(currencyCode), "Currency Code must be specified");
            }

            var currency = currencyLookup.FindCurrency(currencyCode);
            if (!currency.InUse)
            {
                throw new ArgumentException($"Currency {currencyCode} is not valid.");
            }

            if (decimal.Round(amount, currency.DecimalPlaces) != amount)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), $"Amount in {currencyCode} cannot have more than {currency.DecimalPlaces} decimals.");
            }

            Amount = amount;
            Currency = currency;
        }

        protected Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }
        public Currency Currency { get; }


        // public Money Add(Money summand) => new Money(Amount + summand.Amount); 

        public Money Add(Money summand)
        {
            if (Currency != summand.Currency)
            {
                throw new CurrencyMismatchException("Cannot sum amounts with difference currencies");
            }

            return new Money(Amount + summand.Amount, Currency);
        }

        // public Money Subtract(Money subtrahend) => new Money(Amount - subtrahend.Amount);

        public Money Subtract(Money subtrahend)
        {
            if (Currency != subtrahend.Currency)
            {
                throw new CurrencyMismatchException("Cannot subtract amounts with different currencies");
            }

            return new Money(Amount - subtrahend.Amount, Currency);
        }

        public static Money operator +(Money summand1, Money summand2) => summand1.Add(summand2);
        public static Money operator -(Money minuend, Money subtrahend) => minuend.Subtract(subtrahend);

        public override string ToString() => $"{Currency.CurrencyCode} {Amount}"; 


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
        }

        // This equals stuff should be placed in an abstract class. 
        /*

        public bool Equals(Money other)
        {
            if (ReferenceEquals(null, other)) return false; 
            if (ReferenceEquals(this, other)) return true; 
            return Amount.Equals(other.Amount); 
        }

        public override bool Equals(object obj) 
        {
            if (ReferenceEquals(null, obj)) return false; 
            if (ReferenceEquals(this, obj)) return true; 

            if(obj.GetType() != this.GetType()) return false;
            return Equals((Money) obj);  
        }

        public override int GetHashCode() => Amount.GetHashCode(); 

        public static bool operator ==(Money left, Money right) => Equals(left, right); 
        public static bool operator !=(Money left, Money right) => !Equals(left, right);

         */
    }

    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(string message) :
          base(message)
        {
        }
    }
}