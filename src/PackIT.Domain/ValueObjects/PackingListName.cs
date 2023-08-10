using PackIT.Domain.Exceptions;

namespace PackIT.Domain.ValueObjects
{
    public record PackingListName : IEquatable<PackingListName>
    {
        public string Value { get; } // Immutable by nature

        public PackingListName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new EmptyPackingListNameException();
            }

            Value = value;
        }

        public static implicit operator string(PackingListName value)
            => value.Value;

        public static implicit operator PackingListName(string name)
            => new(name);
    }
}
