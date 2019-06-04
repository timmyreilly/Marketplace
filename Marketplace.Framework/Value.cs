using System;

// using an abstract base class for Equatable comparisons to enforce "Value Objects" : https://learning.oreilly.com/library/view/hands-on-domain-driven/9781788834094/3329696a-e003-4603-aa60-3dd4ea028cd9.xhtml 

namespace Marketplace.Framework
{
    public class Value : IEquatable<Value>
    {
        
        public bool Equals(Value other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return true; 
        }
    }
}