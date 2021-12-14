using System.Threading.Tasks;

namespace eShop.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}