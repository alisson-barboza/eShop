using eShop.Core.Messages;
using System;
using System.Collections.Generic;

namespace eShop.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }
        private readonly List<Event> _events;
        public IReadOnlyCollection<Event> Events => _events?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
            _events = new List<Event>();
        }

        public void AddEvent(Event @event)
        {
            _events.Add(@event);
        }

        public void RemoveEvent(Event @event)
        {
            _events.Remove(@event);
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}