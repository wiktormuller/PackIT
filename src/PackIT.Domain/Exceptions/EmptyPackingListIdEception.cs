using PackIT.Shared.Abstractions.Exceptions;

namespace PackIT.Domain.Exceptions
{
    public class EmptyPackingListIdEception : PackItException
    {
        public EmptyPackingListIdEception() : base("Packing list ID cannot be empty.")
        {
        }
    }
}
