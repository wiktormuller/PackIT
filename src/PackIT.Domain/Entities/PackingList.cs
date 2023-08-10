using PackIT.Domain.Events;
using PackIT.Domain.Exceptions;
using PackIT.Domain.ValueObjects;
using PackIT.Shared.Abstractions.Domain;

namespace PackIT.Domain.Entities
{
    public class PackingList : AggregateRoot<PackingListId>
    {
        public Guid Id { get; private set; }

        private PackingListName _name; // Implementations details
        private Localization _localization;

        // When we modify PackingItem (we cannot modify it, because it's VO) we don't want to put the new item at the bottom of the List
        private readonly LinkedList<PackingItem> _items = new();

        // The problem with primitives is that they are lack of behavior.
        // It will be used only via ORM
        internal PackingList(PackingListId id, PackingListName name, Localization localization, LinkedList<PackingItem> items)
            : this(id, name, localization)
        {
            _items = items;
        }

        // It will be used by factory
        internal PackingList(PackingListId id, PackingListName name, Localization localization)
        {
            Id = id;
            _name = name;
            _localization = localization;
        }

        public void AddItem(PackingItem item)
        {
            var alreadyExists = _items.Any(i => i.Name == item.Name);

            if (alreadyExists)
            {
                throw new PackingItemAlreadyExistsException(_name, item.Name);
            }

            _items.AddLast(item);
            AddEvent(new PackingItemAdded(this, item));
        }

        public void AddItems(IEnumerable<PackingItem> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public void PackItem(string itemName)
        {
            var item = GetItem(itemName);

            // It could be done better via method to clone, because with is only swallow copy and don't protect our invariants
            var packedItem = item with { IsPacked = true };

            _items.Find(item).Value = packedItem;
            AddEvent(new PackingItemPacked(this, item));
        }

        public void RemoveItem(string itemName)
        {
            var item = GetItem(itemName);
            _items.Remove(item);
            AddEvent(new PackingItemRemoved(this, item));
        }

        private PackingItem GetItem(string itemName)
        {
            var item = _items.SingleOrDefault(i => i.Name == itemName);

            if (item is null)
            {
                throw new PackingItemNotFoundException(itemName);
            }

            return item;
        }
    }
}
