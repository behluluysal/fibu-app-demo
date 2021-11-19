using System.Threading.Tasks;

namespace App.Repository
{
    public interface IConnectionTestRepository
    {
        Task<dynamic> GetAsync();
    }
}