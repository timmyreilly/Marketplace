using System;
using System.Collections.Generic;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Value
    {
        public ClassifiedAdId Id { get; private set; }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId) 
        {
            Id = id; 
            _ownerId = ownerId; 
        }

        // making the implicit explicit! Refactoring the argument exceptions into value objects. 

        public void SetTitle(string title) => _title = title; 
        public void UpdateText(string text) => _text = text; 
        public void UpdatePrice(decimal price) => _price = price;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id; 
        }

        private UserId _ownerId;
        private string _title; 
        private string _text; 
        private decimal _price;  
    }
}