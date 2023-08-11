namespace PackIT.Application.Services
{
    public interface IPackingReadService
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
