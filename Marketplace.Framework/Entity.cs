using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Framework
{
    public abstract class Entity
    {
        private readonly List<object> _events;
        protected Entity() => _events = new List<object>(); 

        // protected void Raise(object @event) => _events.Add(@event); 

        protected void Apply(object @event) 
        {
            When(@event);
            EnsureValidState();
            _events.Add(@event); 
        }

        protected abstract void When(object @event); 

        public IEnumerable<object> GetChanges() => _events.AsEnumerable();  
        
        public void ClearChanges() => _events.Clear();

        protected abstract void EnsureValidState();  
    }
}