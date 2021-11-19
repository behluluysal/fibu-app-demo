using System.Threading.Tasks;

namespace App.Repository
{
    public interface ITokenRepository
    {
        Task<string> GetToken();
        Task SetToken(string token);
    }
}