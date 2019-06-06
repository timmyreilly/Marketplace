using System;
using System.Collections.Generic;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdId : Value 
    {
        private readonly Guid _value;

        public ClassifiedAdId(Guid value) 
        {
            if (value == null) 
            {
                throw new ArgumentException(nameof(value), "Classified ad id cannot be null"); 
            }

            _value = value; 
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }

        public static implicit operator Guid(ClassifiedAdId self) => self._value; 
    }
}