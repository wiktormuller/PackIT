using PackIT.Domain.ValueObjects;

namespace PackIT.Domain.Policies.Temperature
{
    internal sealed class LowTemperaturePolicy : IPackingItemsPolicy
    {
        public IEnumerable<PackingItem> GenerateItems(PolicyData data)
        {
            return new List<PackingItem>
            {
                new ("Winter hat", 1),
                new ("Scarf", 1),
                new ("Gloves", 1),
                new ("Hoodie", 1),
                new ("Warm jacket", 1)
            };
        }

        public bool IsApplicable(PolicyData data)
        {
            return data.Temperature < 10D;
        }
    }
}
