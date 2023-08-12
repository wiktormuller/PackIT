using PackIT.Domain.Entities;
using PackIT.Domain.Events;
using PackIT.Domain.Exceptions;
using PackIT.Domain.Factories;
using PackIT.Domain.Policies;
using PackIT.Domain.ValueObjects;
using Shouldly;

namespace PackIT.UnitTests.Domain
{
    public class PackingListTests
    {
        private readonly IPackingListFactory _factory;

        public PackingListTests()
        {
            _factory = new PackingListFactory(Enumerable.Empty<IPackingItemsPolicy>());
        }

        [Fact]
        public void AddItem_Throws_PackingItemAlreadyExistsException_When_There_Is_Already_Item_With_The_Same_Name()
        {
            // Arrange
            var packingList = GetPackingList();
            packingList.AddItem(new PackingItem("Item 1", 1));

            // Act
            var exception = Record.Exception(() => packingList.AddItem(new PackingItem("Item 1", 1)));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<PackingItemAlreadyExistsException>();
        }

        [Fact]
        public void AddItem_Adds_PackingItemAdded_Domain_Event_On_Success()
        {
            // Arrange
            var packingList = GetPackingList();

            // Act
            packingList.AddItem(new PackingItem("Item 1", 1));

            // Assert
            packingList.Events.Count().ShouldBe(1);
            var @event = packingList.Events.FirstOrDefault();

            @event.ShouldBeOfType<PackingItemAdded>();
            ((PackingItemAdded)@event).PackingItem.Name.ShouldBe("Item 1");
        }

        private PackingList GetPackingList()
        {
            var packingList = _factory.Create(Guid.NewGuid(), "MyList", Localization.Create("Warsaw, Poland"));
            packingList.ClearEvents();
            return packingList;
        }
    }
}
