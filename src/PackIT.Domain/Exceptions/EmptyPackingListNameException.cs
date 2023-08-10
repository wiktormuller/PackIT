using PackIT.Shared.Abstractions.Exceptions;

namespace PackIT.Domain.Exceptions
{
    internal class EmptyPackingListNameException : PackItException
    {
        public EmptyPackingListNameException() : base("Packing list name cannot be empty.")
        {
        }
    }
}
