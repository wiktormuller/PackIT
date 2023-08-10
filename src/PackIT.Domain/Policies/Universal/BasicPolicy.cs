using PackIT.Domain.ValueObjects;

namespace PackIT.Domain.Policies.Universal
{
    internal sealed class BasicPolicy : IPackingItemsPolicy
    {
        private const uint MaximumQuantityOfClothes = 7;

        public IEnumerable<PackingItem> GenerateItems(PolicyData data)
        {
            return new List<PackingItem>
            {
                new ("Pants", Math.Min(data.Days, MaximumQuantityOfClothes)),
                new ("Socks", Math.Min(data.Days, MaximumQuantityOfClothes)),
                new ("T-Shirt", Math.Min(data.Days, MaximumQuantityOfClothes)),
                new ("Trousers", data.Days < 7 ? 1u : 2u),
                new ("Shampoo", 1),
                new ("Toothbrush", 1),
                new ("Toothpaste", 1),
                new ("Towel", 1),
                new ("Bag pack", 1),
                new ("Passport", 1),
                new ("Phone Charger", 1)
            };
        }

        public bool IsApplicable(PolicyData _)
        {
            return true;
        }
    }
}
