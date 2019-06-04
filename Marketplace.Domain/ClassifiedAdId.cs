using System;

namespace Marketplace.Domain
{
    public class ClassifiedAdId
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
    }
}