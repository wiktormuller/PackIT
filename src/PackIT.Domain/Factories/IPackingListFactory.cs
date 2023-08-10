using PackIT.Domain.Consts;
using PackIT.Domain.Entities;
using PackIT.Domain.ValueObjects;

namespace PackIT.Domain.Factories
{
    public interface IPackingListFactory
    {
        PackingList Create(PackingListId id, PackingListName name, Localization localization);

        // The temperature will be taken from external dependency and will be orchestrated in application layer,
        // because we want to keep our domain factories synchronous and do not make any I/O calls
        PackingList CreateWithDefaultItems(PackingListId id, PackingListName name, TravelDays days, Gender gender,
            Localization localization, Temperature temperature);
    }
}
