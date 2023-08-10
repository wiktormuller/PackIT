using PackIT.Domain.ValueObjects;

namespace PackIT.Domain.Policies.Gender
{
    internal sealed class FemaleGenderPolicy : IPackingItemsPolicy
    {
        public IEnumerable<PackingItem> GenerateItems(PolicyData data)
        {
            return new List<PackingItem>
            {
                new ("Lipstick", 1),
                new ("Powder", 10),
                new ("Eyeliner", 1)
            };
        }

        public bool IsApplicable(PolicyData data)
        {
            return data.Gender is Consts.Gender.Female;
        }
    }
}
